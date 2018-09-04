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
        if (GlobalControl.Instance != null && GlobalControl.Instance.inviteeName != null)
        {
            inviteeName.text = GlobalControl.Instance.inviteeName;
        }
        else
        {
            inviteeName.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.date != null)
        {
            date.text = GlobalControl.Instance.date;
        }
        else
        {
            date.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.message != null)
        {
            message.text = GlobalControl.Instance.message;
        }
        else
        {
            message.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.functionName != null)
        {
            functionName.text = GlobalControl.Instance.functionName;
        }
        else
        {
            functionName.alpha = 0;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.time != null)
        {
            time.text = GlobalControl.Instance.time;
        }
        else
        {
            time.alpha = 0;
        }
    }

}
