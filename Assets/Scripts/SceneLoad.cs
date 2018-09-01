using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour {

    int Index;
    
    void Start()
    {
        Index = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(Index - 1);
    }
}
