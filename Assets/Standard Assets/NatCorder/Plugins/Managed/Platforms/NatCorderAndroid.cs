/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using NatCamU.Dispatch;
    using FramePool = System.Collections.Generic.Dictionary<int, UnityEngine.RenderTexture>;

    public sealed class NatCorderAndroid : AndroidJavaProxy, INatCorder {

        #region --Op vars--
        private VideoFormat videoFormat;
        private RecordingCallback recordingCallback;
        private MainDispatch dispatch;
        private FramePool framePool = new FramePool();
        private readonly AndroidJavaObject natcorder;
        #endregion


        #region --Properties--
        public bool IsRecording {
            get {
                AndroidJNI.AttachCurrentThread();
                return natcorder.Call<bool>("isRecording");
            }
        }
        public bool Verbose { set { natcorder.Call("setVerboseMode", value); }}
        #endregion


        #region --Operations--

        public NatCorderAndroid () : base("com.yusufolokoba.natcorder.NatCorderDelegate") {
            natcorder = new AndroidJavaObject("com.yusufolokoba.natcorder.NatCorder", this, Application.persistentDataPath);
            RenderDispatch.Initialize();
            Debug.Log("NatCorder: Initialized NatCorder 1.3 Android backend");
        }

        public void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback videoCallback) {
            // Make sure that recording size is even
            videoFormat = new VideoFormat(
                videoFormat.width >> 1 << 1,
                videoFormat.height >> 1 << 1,
                videoFormat.framerate,
                videoFormat.bitrate,
                videoFormat.keyframeInterval
            );
            // Save state
            this.dispatch = new MainDispatch();
            this.videoFormat = videoFormat;
            this.recordingCallback = videoCallback;
            // Start recording
            natcorder.Call("startRecording",
                (int)container,
                videoFormat.width,
                videoFormat.height,
                videoFormat.framerate,
                videoFormat.bitrate,
                videoFormat.keyframeInterval,
                audioFormat.sampleRate,
                audioFormat.channelCount
            );
        }

        public void StopRecording () {
            natcorder.Call("stopRecording");
        }

        public Frame AcquireFrame () {
            return new Frame(
                RenderTexture.GetTemporary(
                    videoFormat.width,
                    videoFormat.height,
                    24,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Default,
                    1
                ),
                Frame.CurrentTimestamp
            );
        }

        public void CommitFrame (Frame frame) {
            var handle = ((RenderTexture)frame).GetNativeTexturePtr().ToInt32();
            framePool.Add(handle, frame);
            natcorder.Call("encodeFrame", handle, frame.timestamp);
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            AndroidJNI.AttachCurrentThread();
            natcorder.Call("encodeSamples", sampleBuffer, timestamp);
        }
        #endregion


        #region --Callbacks--

        private void onEncode (int frame) {
            dispatch.Dispatch(() => {
                // Release RenderTexture
                var surface = framePool[frame];
                RenderTexture.ReleaseTemporary(surface);
                framePool.Remove(frame);
            });
        }

        private void onVideo (string path) {
            dispatch.Dispatch(() => recordingCallback(path));
            dispatch.Dispose();
            dispatch = null;
        }
        #endregion
    }
}