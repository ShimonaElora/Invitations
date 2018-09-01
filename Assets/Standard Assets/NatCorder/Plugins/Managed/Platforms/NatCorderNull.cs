/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    public sealed class NatCorderNull : INatCorder {

        #region --Properties--
        public bool IsRecording { get { return false; }}
        #endregion


        #region --Operations--

        public NatCorderNull () {
            UnityEngine.Debug.Log("NatCorder: NatCorder 1.3 is not supported on this platform");
        }

        public void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback recordingCallback) {}

        public void StopRecording () {}

        public Frame AcquireFrame () {return null;}

        public void CommitFrame (Frame frame) {}

        public void CommitSamples (float[] sampleBuffer, long timestamp) {}
        #endregion
    }
}