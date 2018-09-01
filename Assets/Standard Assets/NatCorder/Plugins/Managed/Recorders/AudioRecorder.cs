/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core.Recorders {

    using UnityEngine;
    using System;
    using Docs;

    /// <summary>
    /// Recorder for recording game audio from an audio listener, audio source, or both
    /// </summary>
    [Doc(@"AudioRecorder"), AddComponentMenu(""), DisallowMultipleComponent]
    public sealed class AudioRecorder : MonoBehaviour, IRecorder {
        
        #region --Op vars--
        private float[] sampleBuffer, mixedBuffer;
        private bool commit = true, mute = false;
        private AudioRecorder sibling;
        private float siblingMultiplier = 1f;
        #endregion


        #region --Client API--

        /// <summary>
        /// Create an audio recorder for a scene's AudioListener
        /// </summary>
        /// <param name="sceneAudio">Audio listener for the current scene</param>
        [Doc(@"AudioRecorderCreateListener")]
        public static AudioRecorder Create (AudioListener sceneAudio) {
            // Null checking
            if (!sceneAudio) {
                Debug.LogError("NatCorder Error: Cannot create audio recorder with no audio source provided");
                return null;
            }
            return sceneAudio.gameObject.AddComponent<AudioRecorder>();
        }

        /// <summary>
        /// Create an audio recorder for an audio source
        /// </summary>
        /// <param name="audioSource">Audio source to record</param>
        /// <param name="mute">Optional. Mute audio source after recording so that it is not heard in scene</param>
        [Doc(@"AudioRecorderCreateSource")]
        public static AudioRecorder Create (AudioSource audioSource, bool mute = false) {
            // Null checking
            if (!audioSource) {
                Debug.LogError("NatCorder Error: Cannot create audio recorder with no audio source provided");
                return null;
            }
            var recorder = audioSource.gameObject.AddComponent<AudioRecorder>();
            recorder.mute = mute;
            return recorder;
        }

        /// <summary>
        /// Create an audio recorder for recording scene audio along with microphone audio
        /// </summary>
        /// <param name="sceneAudio">Audio listener for the current scene</param>
        /// <param name="micAudio">Audio source playing microphone audio</param>
        /// <param name="micVolumeMultiplier">Optional. Microphone volume multiplier</param>
        [Doc(@"AudioRecorderCreateMixer")]
        public static AudioRecorder Create (AudioListener sceneAudio, AudioSource micAudio, float micVolumeMultiplier = 1f) {
            // Null checking
            if (!sceneAudio || !micAudio) {
                Debug.LogError("NatCorder Error: Cannot create audio recorder with null audio source");
                return null;
            }
            var sceneRecorder = sceneAudio.gameObject.AddComponent<AudioRecorder>();
            var micRecorder = micAudio.gameObject.AddComponent<AudioRecorder>();
            sceneRecorder.sibling = micRecorder;
            sceneRecorder.siblingMultiplier = micVolumeMultiplier;
            micRecorder.commit = false;
            micRecorder.mute = true;
            return sceneRecorder;
        }

        /// <summary>
        /// Stop recording and teardown resources
        /// </summary>
        [Doc(@"AudioRecorderDispose")]
        public void Dispose () {
            if (sibling) AudioRecorder.Destroy(sibling);
            AudioRecorder.Destroy(this);
        }
        #endregion


        #region --Operations--

        private AudioRecorder () {}

        private void OnAudioFilterRead (float[] data, int channels) { // DEPLOY
            sampleBuffer = sampleBuffer ?? new float[data.Length];
            Array.Copy(data, sampleBuffer, data.Length);
            if (mute) Array.Clear(data, 0, data.Length);
            if (commit) {
                if (sibling) {
                    mixedBuffer = mixedBuffer ?? new float[data.Length];
                    for (int i = 0; i < data.Length; i++)
                        mixedBuffer[i] = Mix(
                            sampleBuffer[i],
                            sibling.sampleBuffer[i] * siblingMultiplier
                        );
                    NatCorder.CommitSamples(mixedBuffer, Frame.CurrentTimestamp);
                }
                else NatCorder.CommitSamples(sampleBuffer, Frame.CurrentTimestamp);
            }
        }

        private static float Mix (float sampleA, float sampleB) {
            /**
             * If different signs, return sum
             * If both positive, return sum minus product
             * If both negative, return sum plus product
             */
            float sum = sampleA + sampleB;
            float product = sampleA * sampleB;
            // Product is negative iff different signs
            if (product < 0) return sum;
            // If control reaches here, then signs must be same
            if (sampleA > 0) return sum - product;
            else return sum + product;
        }
        #endregion
    }
}