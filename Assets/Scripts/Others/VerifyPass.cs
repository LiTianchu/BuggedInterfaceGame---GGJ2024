using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VerifyPass : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] Text resultText;
    public Popup popup;
    
    public void ValidateInput()
    {
        string password = inputField.text;
        if (password == "PASSWORD")
        {
            popup.HidePopup();
            resultText.text = "";
        }
        else
        {
            resultText.text = "Wrong PASSWORD";
            resultText.color = Color.red;
        }
    }
}