using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSceneScript : MonoBehaviour {

    public Text inviteeName;
    public Text date;

	// Use this for initialization
	void Start () {
        if (GlobalControl.Instance != null && GlobalControl.Instance.inviteeName != null)
        {
            inviteeName.text = GlobalControl.Instance.inviteeName;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.date != null)
        {
            date.text = GlobalControl.Instance.date;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
