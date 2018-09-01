using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadB : MonoBehaviour {
    Image bar;
    public float maxtime = 5f;
    float timeleft;
    public GameObject readyText;

    void Start () {
        readyText.SetActive(false);
        bar = GetComponent<Image>();
        timeleft = maxtime;
	}

    void Update() {
        if (timeleft > 0) {
        timeleft -= Time.deltaTime;
        bar.fillAmount = timeleft / maxtime;
        }
        else {
            readyText.SetActive(true);
            Time.timeScale = 0;
        }
	}
}
