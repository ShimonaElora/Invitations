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

        float targetaspect = 1f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        Debug.Log(Screen.height + " " + camera.orthographicSize);

        //camera.orthographicSize = Screen.height / 2f;

        float width = camera.orthographicSize * 2f * camera.aspect;

        camera.aspect = 1;

        camera.orthographicSize = width / 2f;

        //camera.pixelRect = new Rect(0, 0, Screen.width, Screen.width);

        // if scaled height is less than current height, add letterbox
        //if (scaleheight < 1.0f)
        //{
        //    Rect rect = camera.rect;

        //    rect.width = scaleheight;
        //    rect.height = 1.0f;
        //    rect.x = 0;
        //    rect.y = (1.0f - scaleheight) / 2.0f;

        //    camera.rect = rect;
        //}
        //else // add pillarbox
        //{
        //    float scalewidth = 1.0f / scaleheight;

        //    Rect rect = camera.rect;

        //    rect.width = scalewidth;
        //    rect.height = 1.0f;
        //    rect.x = (1.0f - scalewidth) / 2.0f;
        //    rect.y = 0;

        //    camera.rect = rect;
        //}

        VideoFormat videoFormat = new VideoFormat(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelWidth, 60);
        NatCorder.StartRecording(Container.MP4, videoFormat, AudioFormat.Unity, OnRecording);
        var videoRecorder = CameraRecorder.Create(GetComponent<Camera>());
        videoRecorder.recordEveryNthFrame = 4;
        var audioRecorder = AudioRecorder.Create(GetComponent<AudioListener>());
        timeLeft = 20f;
        StartCoroutine(startWait());
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
        if (NatCorder.IsRecording /*&& cameraTexture.didUpdateThisFrame*/)
        {
            // Acquire an encoder frame from NatCorder
            //var frame = NatCorder.AcquireFrame();
            // Blit the current camera preview frame to the encoder frame
            //Graphics.Blit(GetComponent<Camera>().targetTexture, frame);
            // Commit the frame to NatCorder for encoding
            //NatCorder.CommitFrame(frame);
        }
        //Time.timeScale = 1;
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft);
        }
        if (timeLeft < 0)
        {
            Debug.Log("here2");
            //NatCorder.StopRecording();
        }
    }

    IEnumerator startWait()
    {
        Debug.Log("here1");
        yield return new WaitForSeconds(20);
        Debug.Log("here");
        NatCorder.StopRecording();
    }
    
}
