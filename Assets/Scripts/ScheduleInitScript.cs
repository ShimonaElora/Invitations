using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleInitScript : MonoBehaviour {

    public TMPro.TMP_InputField date;
    public TMPro.TMP_InputField functionName;
    public TMPro.TMP_InputField time;
    public TMPro.TMP_InputField message;
    public TMPro.TMP_Dropdown music;

    // Use this for initialization
    void Start ()
    {
        if (GlobalControl.Instance != null && GlobalControl.Instance.date != null)
        {
            date.text = GlobalControl.Instance.date;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.functionName != null)
        {
            functionName.text = GlobalControl.Instance.functionName;
        }
        if (GlobalControl.Instance != null && GlobalControl.Instance.time != null)
        {
            time.text = GlobalControl.Instance.time;
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
