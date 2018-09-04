using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSceneScript : MonoBehaviour {

    public TMPro.TextMeshPro inviteeName;
    public TMPro.TextMeshPro date;
    public TMPro.TextMeshPro message;
    public TMPro.TextMeshPro functionName;
    public TMPro.TextMeshPro time;

    // Use this for initialization
    void Start () {
        if (GlobalControl.Instance != null && GlobalControl.Instance.inviteeName != null && GlobalControl.Instance.videoTypeSelected != 2)
        {
            inviteeName.text = GlobalControl.Instance.inviteeName;
            inviteeName.alpha = 1;
        }
        else
        {
            inviteeName.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.date != null && GlobalControl.Instance.videoTypeSelected != 1)
        {
            date.text = GlobalControl.Instance.date;
            date.alpha = 1;
        }
        else
        {
            date.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.message != null && GlobalControl.Instance.videoTypeSelected != 0)
        {
            message.text = GlobalControl.Instance.message;
            message.alpha = 1;
        }
        else
        {
            message.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.functionName != null && GlobalControl.Instance.videoTypeSelected == 2)
        {
            functionName.text = GlobalControl.Instance.functionName;
            functionName.alpha = 1;
        }
        else
        {
            functionName.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.time != null && GlobalControl.Instance.videoTypeSelected == 2)
        {
            time.text = GlobalControl.Instance.time;
            time.alpha = 1;
        }
        else
        {
            time.alpha = 0;
        }

    }

}
