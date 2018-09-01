/* 
*   NatCorder
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatCorderU.Core {

    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using Docs;
    using Display = UnityEngine.Screen;

    #region --Delegates--
    /// <summary>
    /// Delegate type used to provide the path to a recorded video
    /// </summary>
    [Doc(@"RecordingCallback")]
    public delegate void RecordingCallback (
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string path
    );
    #endregion


    #region --Enumerations--
    /// <summary>
    /// Recording container format
    /// </summary>
    [Doc(@"Container")]
    public enum Container : byte {
        /// <summary>
        /// MP4 video
        /// </summary>
        [Doc(@"ContainerMP4", @"ContainerMP4Discussion")]
        MP4 = 1,
        /// <summary>
        /// Animated GIF image
        /// </summary>
        [Doc(@"ContainerGIF", @"ContainerGIFDiscussion")]
        GIF
    }
    #endregion


    #region --Value Types--

    /// <summary>
    /// Value type used to specify video configuration settings
    /// </summary>
    [Doc(@"VideoFormat")]
    public struct VideoFormat : IEquatable<VideoFormat> {
        /// <summary>
        /// Video width
        /// </summary>
        [Doc(@"VideoFormatWidth")]
        public readonly int width;
        /// <summary>
        /// Video height
        /// </summary>
        [Doc(@"VideoFormatHeight")]
        public readonly int height;
        /// <summary>
        /// Video framerate
        /// </summary>
        [Doc(@"VideoFormatFramerate")]
        public readonly int framerate;
        /// <summary>
        /// Video bitrate in bits per second
        /// </summary>
        [Doc(@"VideoFormatBitrate")]
        public readonly int bitrate;
        /// <summary>
        /// Video keyframe interval in seconds
        /// </summary>
        [Doc(@"VideoFormatKeyframes")]
        public readonly int keyframeInterval;
        /// <summary>
        /// Screen recording configuration
        /// </summary>
        [Doc(@"VideoFormatScreen")]
        public static VideoFormat Screen {
            get {
                return new VideoFormat(
                    Display.width,
                    Display.height,
                    Application.targetFrameRate,
                    (int)(960 * 540 * 11.4f),
                    3
                );
            }
        }
        /// <summary>
        /// Create video configuration settings
        /// </summary>
        /// <param name="width">Video width</param>
        /// <param name="height">Video height</param>
        /// <param name="framerate">Video framerate</param>
        [Doc(@"VideoFormatCtor")]
        public VideoFormat (int width, int height, int framerate = 30) : this(width, height, framerate, Screen.bitrate, Screen.keyframeInterval) {}
        /// <summary>
        /// Create video configuration settings
        /// </summary>
        /// <param name="width">Video width</param>
        /// <param name="height">Video height</param>
        /// <param name="framerate">Video framerate</param>
        /// <param name="bitrate">Video bitrate in bits per second</param>
        /// <param name="keyframeInterval">Video keyframe interval in seconds</param>
        [Doc(@"VideoFormatCtor")]
        public VideoFormat (int width, int height, int framerate, int bitrate, int keyframeInterval) {
            this.width = width;
            this.height = height;
            this.framerate = framerate > 0 ? framerate : 30;
            this.bitrate = bitrate;
            this.keyframeInterval = keyframeInterval;
        }

        public override string ToString () {
            return "{ " + string.Format("{0}x{1} @{2}Hz {3}kbps {4}I", width, height, framerate, bitrate / 1024f, keyframeInterval) + " }";
        }
        public bool Equals (VideoFormat other) {
            return 
                other.width == width && 
                other.height == height &&
                other.framerate == framerate &&
                other.bitrate == bitrate &&
                other.keyframeInterval == keyframeInterval;
        }
        public override int GetHashCode () {
            return width ^ height ^ framerate ^ bitrate ^ keyframeInterval;
        }
        public override bool Equals (object obj) {
            return (obj is VideoFormat) && this.Equals((VideoFormat)obj);
        }
        public static bool operator == (VideoFormat lhs, VideoFormat rhs) {
            return lhs.Equals(rhs);
        }
        public static bool operator != (VideoFormat lhs, VideoFormat rhs) {
            return !lhs.Equals(rhs);
        }
    }

    /// <summary>
    /// Value type used to specify audio configuration settings
    /// </summary>
    [Doc(@"AudioFormat")]
    public struct AudioFormat : IEquatable<AudioFormat> {
        /// <summary>
        /// Audio sample rate
        /// </summary>
        [Doc(@"AudioFormatSampleRate")]
        public readonly int sampleRate;
        /// <summary>
        /// Audio channel count
        /// </summary>
        [Doc(@"AudioFormatChannelCount")]
        public readonly int channelCount;
        /// <summary>
        /// Audio format for recording Unity audio
        /// </summary>
        [Doc(@"AudioFormatUnity")]
        public static AudioFormat Unity {
            get {
                return new AudioFormat(AudioSettings.outputSampleRate, (int)AudioSettings.speakerMode);
            }
        }
        /// <summary>
        /// Audio format for not recording any audio
        /// </summary>
        [Doc(@"AudioFormatNone")]
        public static readonly AudioFormat None = new AudioFormat(0, 0);
        /// <summary>
        /// Create audio configuration settings
        /// </summary>
        /// <param name="sampleRate">Audio sample rate</param>
        /// <param name="channelCount">Audio channel count</param>
        [Doc(@"AudioFormatCtor")]
        public AudioFormat (int sampleRate, int channelCount) {
            this.sampleRate = sampleRate;
            this.channelCount = channelCount;
        }

        public override string ToString () {
            return "{ " + string.Format("{0}@{1}Hz", channelCount, sampleRate) + " }";
        }
        public bool Equals (AudioFormat other) {
            return other.channelCount == channelCount && other.sampleRate == sampleRate;
        }
        public override int GetHashCode () {
            return sampleRate ^ channelCount;
        }
        public override bool Equals (object obj) {
            return (obj is AudioFormat) && this.Equals((AudioFormat)obj);
        }
        public static bool operator == (AudioFormat lhs, AudioFormat rhs) {
            return lhs.Equals(rhs);
        }
        public static bool operator != (AudioFormat lhs, AudioFormat rhs) {
            return !lhs.Equals(rhs);
        }
    }

    /// <summary>
    /// Encoder surface for recording a video frame
    /// </summary>
    [Doc(@"Frame")]
    public sealed class Frame { // We make it a class instead of struct so we get reference equality
        
        /// <summary>
        /// Timestamp for this moment
        /// </summary>
        [Doc(@"FrameCurrentTimestamp")]
        public static long CurrentTimestamp {
            get {
                return (long)(((double)System.Diagnostics.Stopwatch.GetTimestamp() / System.Diagnostics.Stopwatch.Frequency) * 1e+9f);
            }
        }
        /// <summary>
        /// Frame timestamp in nanoseconds
        /// </summary>
        [Doc(@"FrameTimestamp")]
        public long timestamp = -1;
        private readonly RenderTexture surface;

        /// <summary>
        /// DO NOT USE.
        /// </summary>
        public Frame (RenderTexture surface, long timestamp) {
            this.surface = surface;
            this.timestamp = timestamp;
        }

        public static implicit operator RenderTexture (Frame frame) {
            return frame.surface;
        }
    }
    #endregion
}