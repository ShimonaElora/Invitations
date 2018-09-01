using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLimitScript : MonoBehaviour {

    public int limit;
    public TMPro.TMP_InputField inputField;

    private TextMesh text;
    private string textString;
    private int currentValue;

	// Use this for initialization
	void Start () {
        currentValue = limit;
        text = GetComponent<TextMesh>();
        textString = text.text.ToString();
        textString = textString.Substring(0, textString.IndexOf("("));
        inputField.onValueChanged.AddListener(delegate
        {
            onTextChange();
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onTextChange()
    {
        currentValue = limit - inputField.text.Length;
        if (currentValue >= 0)
        {
            text.text = textString + "(" + currentValue + ")";
        } 
        else
        {
            inputField.text = inputField.text.ToString().Substring(0, inputField.text.Length - 1);
        }
    }
}
