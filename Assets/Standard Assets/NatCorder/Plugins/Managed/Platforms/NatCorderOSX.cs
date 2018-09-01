/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

#define DEFERRED_READBACK // Saves some time at the cost of memory

namespace NatCorderU.Core.Platforms {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using NatCamU.Dispatch;

    public class NatCorderOSX : INatCorder {

        #region --Op vars--
        protected VideoFormat videoFormat;
        protected RecordingCallback recordingCallback;
        protected Texture2D framebuffer;
        private MainDispatch dispatch;
        private static NatCorderOSX instance { get { return NatCorder.Implementation as NatCorderOSX; }}
        #endregion


        #region --Properties--
        public bool IsRecording { get { return NatCorderBridge.IsRecording(); }}
        #endregion


        #region --Operations--

        public NatCorderOSX () {
            var writePath = 
            #if UNITY_EDITOR
            System.IO.Directory.GetCurrentDirectory();
            #else
            Application.persistentDataPath;
            #endif
            NatCorderBridge.Initialize(null, OnVideo, writePath);
            Debug.Log("NatCorder: Initialized NatCorder 1.3 macOS backend");
        }

        public virtual void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback recordingCallback) {
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
            this.recordingCallback = recordingCallback;
            this.framebuffer = new Texture2D(videoFormat.width, videoFormat.height, TextureFormat.ARGB32, false);
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

        public virtual void StopRecording () {
            NatCorderBridge.StopRecording();
            Texture2D.Destroy(framebuffer);
            framebuffer = null;
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

        public virtual void CommitFrame (Frame frame) {            
            // Submit
            #if DEFERRED_READBACK
            dispatch.Dispatch(() => {
                if (dispatch != null) dispatch.Dispatch(() => { // Defer readback for (at least) one frame
                    if (!IsRecording) {
                        RenderTexture.ReleaseTemporary(frame);
                        return;
                    }
            #endif
                    var currentRT = RenderTexture.active;
                    RenderTexture.active = frame;
                    framebuffer.ReadPixels(new Rect(0, 0, videoFormat.width, videoFormat.height), 0, 0, false);
                    RenderTexture.active = currentRT;
                    RenderTexture.ReleaseTemporary(frame);            
                    var pixelBuffer = framebuffer.GetRawTextureData();
                    var bufferHandle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
                    NatCorderBridge.EncodeFrame(bufferHandle.AddrOfPinnedObject(), frame.timestamp);
                    bufferHandle.Free();
            #if DEFERRED_READBACK
                });
                else RenderTexture.ReleaseTemporary(frame);
            });
            #endif
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            NatCorderBridge.EncodeSamples(sampleBuffer, sampleBuffer.Length, timestamp);
        }
        #endregion


        #region --Callbacks--

        [MonoPInvokeCallback(typeof(RecordingCallback))]
        private static void OnVideo (
            #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            [MarshalAs(UnmanagedType.LPWStr)]
            #endif
            string path
        ) {
            instance.dispatch.Dispatch(() => instance.recordingCallback(path));
            instance.dispatch.Dispose();
            instance.dispatch = null;
        }
        #endregion
    }
}