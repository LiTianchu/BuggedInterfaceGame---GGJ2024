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
                StartCoroutine(Wrong());                
            }
        }
        else
        {
            hintText.text = "not this one";
        }
    }

    IEnumerator Wrong()
    {
        resultText.text = "That is not the PASSWORD";
        resultText.color = Color.red;
        yield return new WaitForSeconds(2);
        resultText.text = "";
    }
}
