using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public string inviteeName;
    public string date;
    public int languageSelected;
    public int videoTypeSelected;
    public bool addIntro;
    public string message;
    public string functionName;
    public string time;
    public int music;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        inviteeName = "";
        date = "";
        languageSelected = 0;
        videoTypeSelected = 0;
        music = 0;
    }

}
