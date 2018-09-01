using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class NextButtonScript : MonoBehaviour {

    public Toggle addIntro;
    public TMPro.TMP_InputField inviteeName;
    public TMPro.TMP_InputField date;
    public TMPro.TMP_InputField message;
    public TMPro.TMP_InputField functionName;
    public TMPro.TMP_InputField time;
    public TMPro.TMP_Dropdown music;

    public Canvas canvas;

    private Button buttonNext;

	// Use this for initialization
	void Start () {
        buttonNext = GetComponent<Button>();
        buttonNext.onClick.AddListener(onButtonNextClick);
	}

    void onButtonNextClick()
    {
        if (addIntro != null)
        {
            GlobalControl.Instance.addIntro = addIntro.isOn;
        }
        if (inviteeName != null)
        {
            if (inviteeName.text != null && inviteeName.text.Length != 0)
                GlobalControl.Instance.inviteeName = inviteeName.text.ToString();
        }
        if (date != null)
        {
            if (date.text != null && date.text.Length != 0)
                GlobalControl.Instance.date = date.text.ToString();
        }
        if (message != null)
        {
            if (message.text != null && message.text.Length != 0)
                GlobalControl.Instance.message = message.text.ToString();
        }
        if (functionName != null)
        {
            if (functionName.text != null && functionName.text.Length != 0)
                GlobalControl.Instance.functionName = functionName.text.ToString();
        }
        if (time != null)
        {
            if (time.text != null && time.text.Length != 0)
                GlobalControl.Instance.time = time.text.ToString();
        }
        if (music != null)
        {
            //GlobalControl.Instance.music = music.options[music.value].text.ToString();
        }

        SceneManager.LoadScene("Animation Scene1");
        canvas.enabled = false;

    }

}
