using UnityEngine.UI;
using UnityEngine;

public class ReadFromPreviousValueScript : MonoBehaviour {

    public Toggle englishToggle;
    public Toggle hindiToggle;
    public Toggle invitationToggle;
    public Toggle greetingToggle;
    public Toggle scheduleToggle;

    // Use this for initialization
    void Start () {
		if (GlobalControl.Instance != null)
        {
            if (GlobalControl.Instance.languageSelected == 0)
            {
                englishToggle.isOn = true;
                hindiToggle.isOn = false;
            }
            else
            {
                englishToggle.isOn = false;
                hindiToggle.isOn = true;
            }

            switch (GlobalControl.Instance.videoTypeSelected)
            {
                case 0:
                    invitationToggle.isOn = true;
                    greetingToggle.isOn = false;
                    scheduleToggle.isOn = false;
                    break;
                case 1:
                    invitationToggle.isOn = false;
                    greetingToggle.isOn = true;
                    scheduleToggle.isOn = false;
                    break;
                case 2:
                    invitationToggle.isOn = false;
                    greetingToggle.isOn = false;
                    scheduleToggle.isOn = true;
                    break;
            }
        }

        englishToggle.onValueChanged.AddListener(delegate
        {
            onEnglishClicked();
        });
        hindiToggle.onValueChanged.AddListener(delegate
        {
            onHindiClicked();
        });
        invitationToggle.onValueChanged.AddListener(delegate
        {
            onInvitationClicked();
        });
        greetingToggle.onValueChanged.AddListener(delegate
        {
            onGreetingClicked();
        });
        scheduleToggle.onValueChanged.AddListener(delegate
        {
            onScheduleClicked();
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void onEnglishClicked()
    {
        GlobalControl.Instance.languageSelected = 0;
    }

    void onHindiClicked()
    {
        GlobalControl.Instance.languageSelected = 1;
    }

    void onInvitationClicked()
    {
        GlobalControl.Instance.videoTypeSelected = 0;
    }

    void onGreetingClicked()
    {
        GlobalControl.Instance.videoTypeSelected = 1;
    }

    void onScheduleClicked()
    {
        GlobalControl.Instance.videoTypeSelected = 2;
    }
}
