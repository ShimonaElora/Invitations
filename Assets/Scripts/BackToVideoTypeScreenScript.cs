using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToVideoTypeScreenScript : MonoBehaviour {

    private Button backButton;

	// Use this for initialization
	void Start () {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(onBackClick);
    }
	
    void onBackClick()
    {
        if (GlobalControl.Instance != null)
        {
            switch (GlobalControl.Instance.videoTypeSelected)
            {
                case 0:
                    SceneManager.LoadScene("Invitation Scene");
                    break;
                case 1:
                    SceneManager.LoadScene("Greeting Scene");
                    break;
                case 2:
                    SceneManager.LoadScene("Schedule Scene");
                    break;
            }
        }
    }

}
