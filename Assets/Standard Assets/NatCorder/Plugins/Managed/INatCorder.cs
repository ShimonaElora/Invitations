/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Platforms {

    public interface INatCorder {

        #region --Properties--
        bool IsRecording { get; }
        #endregion
        
        #region --Operations--
        void StartRecording (Container container, VideoFormat videoFormat, AudioFormat audioFormat, RecordingCallback recordingCallback);
        void StopRecording ();
        Frame AcquireFrame ();
        void CommitFrame (Frame frame);
        void CommitSamples (float[] sampleBuffer, long timestamp);
        #endregion
    }
}