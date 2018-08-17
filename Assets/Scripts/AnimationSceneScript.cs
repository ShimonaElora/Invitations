using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSceneScript : MonoBehaviour {

    public Text inviteeName;
    public Text date;

	// Use this for initialization
	void Start () {
        inviteeName.text = GlobalControl.Instance.inviteeName;
        date.text = GlobalControl.Instance.date;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
