using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class DisableShare : MonoBehaviour
{
    public int timeLeft = 30;
    public GameObject Button;
    public GameObject DisButton;

    void Start()
    {
        StartCoroutine("LoseTime");
        Time.timeScale = 1; 
    }
    void Update()
    {
        if (timeLeft <= 1)
        {
            DisButton.SetActive(false);
            Button.SetActive(true);
        }
        else
        {
            DisButton.SetActive(true);
            Button.SetActive(false);
        }
    }
    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

}