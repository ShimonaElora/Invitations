using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatCorderU.Core;
using NatCorderU.Core.Recorders;
using System;

public class RecordCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //VideoFormat videoFormat = new VideoFormat(Camera.main.pixelWidth, Camera.main.pixelHeight);
        //NatCorder.StartRecording(Container.MP4, new VideoFormat(Camera.main.pixelWidth, Camera.main.pixelHeight), AudioFormat.Unity, OnRecording);
        //var videoRecorder = CameraRecorder.Create(Camera.main);
        //var audioRecorder = AudioRecorder.Create(GetComponent<AudioListener>());
    }

    //private void OnRecording(string path)
    //{
    //    throw new NotImplementedException();
    //}

    // Update is called once per frame
    void Update () {
        //if (NatCorder.IsRecording /*&& cameraTexture.didUpdateThisFrame*/)
        //{
        //    // Acquire an encoder frame from NatCorder
        //    var frame = NatCorder.AcquireFrame();
        //    // Blit the current camera preview frame to the encoder frame
        //    //Graphics.Blit(cameraTexture, frame);
        //    // Commit the frame to NatCorder for encoding
        //    NatCorder.CommitFrame(frame);
        //}
    }
    
}
