using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Change : MonoBehaviour
{
    [SerializeField] string username;
    [SerializeField] string password;
    [SerializeField] HackedInputField usernameField; 
    [SerializeField] HackedInputField passwordField;
    [SerializeField] GameObject deactivated;
    [SerializeField] GameObject activated;

    public void Replace()
    {
        if (usernameField.text == username && passwordField.text == password)
        {
            deactivated.SetActive(false);
            activated.SetActive(true);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
