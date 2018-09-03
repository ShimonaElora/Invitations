using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatCorderU.Core;
using NatCorderU.Core.Recorders;
using System;

public class RecordCamera : MonoBehaviour {

    private float timeLeft = 20f;

	// Use this for initialization
	void Start () {
        //VideoFormat videoFormat = new VideoFormat(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight);
        //NatCorder.StartRecording(Container.MP4, videoFormat, AudioFormat.Unity, OnRecording);
        //var videoRecorder = CameraRecorder.Create(GetComponent<Camera>());
        //videoRecorder.recordEveryNthFrame = 4;
        //var audioRecorder = AudioRecorder.Create(GetComponent<AudioListener>());
        //timeLeft = 20f;
        //StartCoroutine(startWait());
    }

    private void OnRecording(string path)
    {
        //throw new NotImplementedException();
        Debug.Log("here");
        //Handheld.PlayFullScreenMovie(path);
        NatShareU.NatShare.ShareMedia(path);
    }

    //Update is called once per frame
    void Update () {
        //if (NatCorder.IsRecording /*&& cameraTexture.didUpdateThisFrame*/)
        //{
        //    // Acquire an encoder frame from NatCorder
        //    var frame = NatCorder.AcquireFrame();
        //    // Blit the current camera preview frame to the encoder frame
        //    Graphics.Blit(GetComponent<Camera>().targetTexture, frame);
        //    // Commit the frame to NatCorder for encoding
        //    NatCorder.CommitFrame(frame);
        //}
        ////Time.timeScale = 1;
        //if (timeLeft > 0)
        //{
        //    timeLeft -= Time.deltaTime;
        //    Debug.Log(timeLeft);
        //}
        //if (timeLeft < 0)
        //{
        //    Debug.Log("here2");
        //    //NatCorder.StopRecording();
        //}
    }

    IEnumerator startWait()
    {
        Debug.Log("here1");
        yield return new WaitForSeconds(20);
        Debug.Log("here");
        NatCorder.StopRecording();
    }
    
}
