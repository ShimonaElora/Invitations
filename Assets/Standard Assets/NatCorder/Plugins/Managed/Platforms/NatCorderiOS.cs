/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using AOT;
    using UnityEngine;
    using System;
    using NatCamU.Dispatch;
    using FramePool = System.Collections.Generic.Dictionary<System.IntPtr, UnityEngine.RenderTexture>;

    public sealed class NatCorderiOS : INatCorder {

        #region --Op vars--
        private VideoFormat videoFormat;
        private RecordingCallback recordingCallback;
        private MainDispatch dispatch;
        private static FramePool framePool = new FramePool();
        private static NatCorderiOS instance { get { return NatCorder.Implementation as NatCorderiOS; }}
        #endregion
        

        #region --Properties--
        public bool IsRecording { get { return NatCorderBridge.IsRecording();}}
        #endregion


        #region --Operations--

        public NatCorderiOS () {
            NatCorderBridge.Initialize(OnEncode, OnVideo, Application.persistentDataPath);
            RenderDispatch.Initialize();
            Debug.Log("NatCorder: Initialized NatCorder 1.3 iOS backend");
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
            NatCorderBridge.StartRecording(
                container,
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
            NatCorderBridge.StopRecording();
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
            var handle = ((RenderTexture)frame).GetNativeTexturePtr();
            framePool.Add(handle, frame);
            NatCorderBridge.EncodeFrame(handle, frame.timestamp);
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            NatCorderBridge.EncodeSamples(sampleBuffer, sampleBuffer.Length, timestamp);
        }
        #endregion


        #region --Callbacks--

        [MonoPInvokeCallback(typeof(NatCorderBridge.EncodeCallback))]
        private static void OnEncode (IntPtr frame) {
            instance.dispatch.Dispatch(() => {
                // Release RenderTexture
                var surface = framePool[frame];
                RenderTexture.ReleaseTemporary(surface);
                framePool.Remove(frame);
            });
        }

        [MonoPInvokeCallback(typeof(RecordingCallback))]
        private static void OnVideo (string path) {
            instance.dispatch.Dispatch(() => instance.recordingCallback(path));
            instance.dispatch.Dispose();
            instance.dispatch = null;
        }
        #endregion
    }
}