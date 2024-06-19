using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VerifyPass : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] bool actual;
    [SerializeField] Text resultText;
    [SerializeField] Text hintText;
    public Popup popup;
    
    public void ValidateInput()
    {
        if (actual)
        {
            string password = inputField.text;
            if (password == "PASSWORD" || password == "password")
            {
                inputField.text = "";
                resultText.text = "";
                popup.HidePopup();
            }
            else
            {
                resultText.text = "Instructions not followed. Request rejected :D";
                resultText.color = Color.red;
            }
        }
        else
        {
            hintText.text = "not this one";
        }
    }
}
