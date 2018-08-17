using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ValidationScript : MonoBehaviour {

    public InputField inviteeName;
    public InputField date;
    public Button playButton;

	// Use this for initialization
	void Start () {
		playButton.onClick.AddListener(changeScene);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void changeScene()
    {
        if (inviteeName.text.ToString().Trim().Length != 0 && date.text.ToString().Length != 0)
        {
            Debug.Log("here");
            GlobalControl.Instance.inviteeName = inviteeName.text.ToString().Trim();
            GlobalControl.Instance.date = date.text.ToString().Trim();
            SceneManager.LoadScene("Animation Scene", LoadSceneMode.Single);
        }
    }
}
