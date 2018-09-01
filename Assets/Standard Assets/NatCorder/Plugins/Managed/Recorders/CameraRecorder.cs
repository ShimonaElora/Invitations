/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Recorders {

    using UnityEngine;
    using System;
    using Docs;

    /// <summary>
    /// Recorder for recording a game camera
    /// </summary>
    [Doc(@"CameraRecorder"), AddComponentMenu(""), DisallowMultipleComponent]
    public class CameraRecorder : MonoBehaviour, IRecorder {
        
        #region --Op vars--
        /// <summary>
        /// Control number of successive camera frames to skip while recording.
        /// This is very useful for GIF recording, which typically has a lower framerate appearance
        /// </summary>
        [Doc(@"CameraRecorderNthFrame", @"CameraRecorderNthFrameDiscussion"), Code(@"RecordGIF")]
        public int recordEveryNthFrame = 1;
        /// <summary>
        /// Material to use for recording the camera view.
        /// This is useful for applying effects while recording.
        /// Set to `null` to record without a material
        /// </summary>
        [Doc(@"CameraRecorderMaterial", @"CameraRecorderMaterialDiscussion"), Code(@"RecordBW")]
        public Material recordingMaterial;
        private int frameCount;
        #endregion


        #region --Client API--

        /// <summary>
        /// Create a camera recorder for a game camera
        /// </summary>
        /// <param name="camera">Game camera to record</param>
        [Doc(@"CameraRecorderCreate"), Code(@"RecordGIF")]
        public static CameraRecorder Create (Camera camera) {
            if (!camera) {
                Debug.LogError("NatCorder Error: Cannot create video recorder with no camera provided");
                return null;
            }
            var recorder = camera.gameObject.AddComponent<CameraRecorder>();
            return recorder;
        }

        /// <summary>
        /// Stop recording and teardown resources
        /// </summary>
        [Doc(@"CameraRecorderDispose")]
        public void Dispose () {
            CameraRecorder.Destroy(this);
            Material.Destroy(recordingMaterial);
        }
        #endregion


        #region --Operations--

        private CameraRecorder () {}

        private void OnRenderImage (RenderTexture src, RenderTexture dst) {
            // Blit to recording frame
            if (NatCorder.IsRecording && frameCount++ % recordEveryNthFrame == 0) {
                var encoderFrame = NatCorder.AcquireFrame();
                if (recordingMaterial)
                    Graphics.Blit(src, encoderFrame, recordingMaterial);
                else
                    Graphics.Blit(src, encoderFrame);
                NatCorder.CommitFrame(encoderFrame);
            }
            // Blit to render pipeline
            Graphics.Blit(src, dst);
        }
        #endregion
    }
}