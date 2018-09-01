/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using System;

    public sealed class NatCorderWindows : NatCorderOSX {

        #region --Op vars--
        private readonly Material transformMat;
        #endregion

        
        #region --Operations--

        public NatCorderWindows () : base() {
            transformMat = new Material(Shader.Find("Hidden/NatCorder/Transform"));
            Debug.Log("NatCorder: Initialized NatCorder 1.3 Windows backend with macOS implementation");
        }

        public override void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback recordingCallback) {
            base.StartRecording(
                container,
                videoFormat,
                audioFormat,
                path => recordingCallback(path.Replace('/', '\\'))
            );
        }

        public override void CommitFrame (Frame frame) {
            // Invert
            var correctedFrame = AcquireFrame();
            correctedFrame.timestamp = frame.timestamp;
            Graphics.Blit(frame, correctedFrame, transformMat);
            RenderTexture.ReleaseTemporary(frame);
            // Commit
            base.CommitFrame(correctedFrame);
        }
        #endregion
    }
}