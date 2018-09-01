# NatCorder API
NatCorder is a lightweight, easy-to-use, native video recording API for iOS and Android. NatCorder comes with a rich featureset including:
+ Record anything that can be rendered into a texture.
+ Control recording quality and file size with bitrate and keyframe interval.
+ Record at any resolution. You get to specify what resolution recording you want.
+ Record GIF's.
+ Get path to recorded video in device storage.
+ Record game audio with video.
+ Support for recording on macOS--in the Editor or in Standalone builds.
+ Support for recording on Windows--in the Editor or in Standalone builds.
+ Experimental support for recording on WebGL.

## Fundamentals of Recording
NatCorder provides a simple recording API with the `NatCorder` class. NatCorder works by encoding video and audio frames on demand. To start recording, you will provide a `Container` format which tells NatCorder what kind of video file you want (it currently supports `MP4` and `GIF`); a `VideoFormat` for specifying video configuration settings; an `AudioFormat` for specifying audio configuration settings; and a `RecordingCallback` which will be invoked with the path to the recorded video file:
```csharp
NatCorder.StartRecording(
    Container.GIF,      // Specify the container format as GIF
    VideoFormat.Screen, // Video configuration like width, height, framerate
    AudioFormat.None,   // Audio configuration like sample rate, channel count
    OnRecording         // Recording callback
);
```

Once `StartRecording` is called, you then commit frames to NatCorder. You can commit video and audio frames to NatCorder.

### Committing Video Frames
To commit video frames for encoding, you need to do three things:
1. Acquire an encoder `Frame` using `NatCorder.AcquireFrame`
2. Blit or render to the encoder `Frame`
3. Commit the `Frame` using `NatCorder.CommitFrame`

Usually, you would perform this process in the game loop, like in `Update` or `OnRenderImage`:
```csharp
WebCamTexture webcamPreview; // Start this somewhere

void Update () {
    // Check that we are recording, and that the webcamtexture was updated this frame
    if (NatCorder.IsRecording && webcamPreview.didUpdateThisFrame) {
        // Acquire an encoder frame
        var frame = NatCorder.AcquireFrame();
        // Blit the webcam preview to the frame
        Graphics.Blit(webcamPreview, frame);
        // Commit the frame to the encoder
        NatCorder.CommitFrame(frame);
    }
}
```

You can also alter the timestamp on the encoder frame so that the generated video is retimed. To do so, set the value on `frame.timestamp` before committing it.

### Committing Audio Frames
To commit audio frames, you simply invoke `NatCorder.CommitSamples` passing in a PCM audio sample buffer (`float[]`) and a corresponding `timestamp`. NatCorder expects that the sample buffer be interleaved if it has more than one channel. Usually, you would commit audio samples in Unity's `OnAudioFilterRead` function:
```csharp
void OnAudioFilterRead (float[] data, int channels) {
    // Check that we are recording
    if (NatCorder.IsRecording) {
        // Get the timestamp for this exact moment
        var timestamp = Frame.CurrentTimestamp;
        // Commit the frame
        NatCorder.CommitSamples(data, timestamp);
    }
}
```

## Easier Recording with Recorders
In most cases, you will likely just want to record a game camera optionally with game audio. To do so, you don't need to manually acquire, blit, and commit frames. Instead, you can use NatCorder's `Recorders`. A `Recorder` is a lightweight utility class that eases out the process of recording some aspect of a Unity application. NatCorder comes with two recorders: `CameraRecorder` and `AudioRecorder`. You can create your own recorders to do more interesting things like add a watermark to the video, or retime the video. Here is a simple example showing recording a game camera:
```csharp
void StartRecording () {
    // Start recording
    NatCorder.StartRecording(Container.MP4, VideoFormat.Screen, AudioFormat.None, OnRecording);
    // Create a camera recorder to record the main camera
    videoRecorder = CameraRecorder.Create(Camera.main);
}

void StopRecording () {
    // Destroy the camera recorder
    videoRecorder.Dispose();
    // Stop recording
    NatCorder.StopRecording();
}
```

Check out the included examples and the documentation for more information about the recorders.

___

## Limitations of the WebGL Backend
The WebGL backend is currently experimental. As a result, it has a few limitations in its operations. Firstly, it is an 'immediate-encode' backend. This means that video frames are encoded immediately they are committed to NatCorder. As a result, there is no support for custom frame timing (the `timestamp` value of a `Frame` is always ignored).

Secondly, because Unity does not support the `OnAudioFilterRead` callback on WebGL, we cannot record game audio on WebGL (using an `AudioSource` or `AudioListener`). This is a limitation of Unity's WebGL implementation. However, you can still record raw audio data using the `NatCorder.CommitSamples` API.

Videos recorded on WebGL may be recorded with the VP8/9 codec or H.264 codec, depending on the browser and device. Videos are always recorded in the `webm` container format.

## Using NatCorder with NatCam
If you use NatCorder with our NatCam camera API, then you will have to remove duplicate copies of libraries that are shared by both API's:
- `Dispatch.dll` in 'NatCorder > Plugins > Managed > Dispatch'
- `RenderDispatch.cs` in 'NatCorder > Plugins > Managed > Dispatch'
- `NatCamRenderPipeline.aar` in 'NatCorder > Plugins > Android'
- `libNatCamRenderPipeline.a` in 'NatCorder > Plugins > iOS'

## Tutorials
- [Unity Recording Made Easy](https://medium.com/@olokobayusuf/natcorder-unity-recording-made-easy-f0fdee0b5055)
- [Audio Workflows](https://medium.com/@olokobayusuf/natcorder-tutorial-audio-workflows-1cfce15fb86a)

## Requirements
- On Android, NatCorder requires API Level 18 and up
- On iOS, NatCorder requires iOS 7 and up
- On macOS, NatCorder requires macOS 10.13 and up
- On Windows, NatCorder requires Windows 8 and up
- On WebGL, NatCorder requires Chrome 47 or Safari 27 and up

## Notes
- NatCorder doesn't have full support for recording UI canvases that are in Screen Space - Overlay mode. See [here](https://forum.unity3d.com/threads/render-a-canvas-to-rendertexture.272754/#post-1804847).
- On Android, it is strongly recommended to enable 'Multithreaded Rendering' in Player Settings. It greatly improves recording performance.
- On iOS, it is strongly recommended to use the Metal graphics API. It greatly improves recording performance.
- When building for WebGL, make sure that 'Use Prebuild Engine' is disabled in Build Settings.
- When recording audio, make sure that the 'Bypass Listener Effects' and 'Bypass Effects' flags on your `AudioSource`s are turned off.
- When manually recording a game camera (using the `NatCorder` API), make sure you are recording in `OnPostRender`, `OnRenderImage`, or after `yield WaitForEndOfFrame()`. Recording a game camera in `Update` will likely produce artifacts in the recorded video.

## Quick Tips
- Please peruse the included scripting reference [here](https://olokobayusuf.github.io/NatCorder-Docs/)
- To discuss or report an issue, visit Unity forums [here](https://forum.unity.com/threads/natcorder-video-recording-api.505146/)
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com)

Thank you very much!