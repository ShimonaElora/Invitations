using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class VideoTypeSceneChangerScript : MonoBehaviour {

    public Toggle invitationToggle;
    public Toggle greetingToggle;
    public Toggle scheduleToggle;

    private Button nextButton;

	// Use this for initialization
	void Start () {
        nextButton = GetComponent<Button>();
        nextButton.onClick.AddListener(onButtonClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onButtonClick()
    {
        if (invitationToggle.isOn)
        {
            SceneManager.LoadScene("Invitation Scene", LoadSceneMode.Single);
        }
        else if (greetingToggle.isOn)
        {
            SceneManager.LoadScene("Greeting Scene", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Schedule Scene", LoadSceneMode.Single);
        }
    }
}
