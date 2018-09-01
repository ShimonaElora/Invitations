using UnityEngine;
using UnityEngine.UI;

public class GreetingInitScript : MonoBehaviour {

    public Toggle addIntro;
    public TMPro.TMP_InputField inviteeName;
    public TMPro.TMP_InputField message;
    public TMPro.TMP_Dropdown music;

    // Use this for initialization
    void Start () {
        if (GlobalControl.Instance != null && GlobalControl.Instance.inviteeName != null)
        {
            inviteeName.text = GlobalControl.Instance.inviteeName;
        }
        if (GlobalControl.Instance != null)
        {
            addIntro.isOn = GlobalControl.Instance.addIntro;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.message != null)
        {
            message.text = GlobalControl.Instance.message;
        }
        if (GlobalControl.Instance != null)
        {
            music.value = GlobalControl.Instance.music;
        }
    }
	
}
