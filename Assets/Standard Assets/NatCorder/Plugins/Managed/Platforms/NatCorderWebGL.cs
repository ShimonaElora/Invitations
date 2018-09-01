/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    using UnityEngine;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using NatCamU.Dispatch;

    public sealed class NatCorderWebGL : NatCorderOSX { // EXPERIMENTAL // Because of Unity audio
        
        #region --Op vars--
        private readonly Material transformMat;        
        #endregion


        #region --Operations--

        public NatCorderWebGL () : base() {
            transformMat = new Material(Shader.Find("Hidden/NatCorder/Transform"));
            transformMat.EnableKeyword(@"PLATFORM_WEBGL");
            Debug.Log("NatCorder: Initialized NatCorder 1.3 WebGL backend with macOS implementation");
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