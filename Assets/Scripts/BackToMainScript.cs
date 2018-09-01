using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BackToMainScript : MonoBehaviour {

    private Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(onBackClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onBackClick()
    {
        SceneManager.LoadScene("Main Scene");
    }

}
