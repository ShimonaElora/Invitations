using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitationInitScript : MonoBehaviour {

    public Toggle addIntro;
    public TMPro.TMP_InputField inviteeName;
    public TMPro.TMP_InputField date;
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
        if (GlobalControl.Instance != null && GlobalControl.Instance.date != null)
        {
            date.text = GlobalControl.Instance.date;
        }
        if (GlobalControl.Instance != null)
        {
            music.value = GlobalControl.Instance.music;
        }
    }

}
