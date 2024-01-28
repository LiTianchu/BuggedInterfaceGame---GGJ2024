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
    [SerializeField] GameObject spawnable;
    [SerializeField] GameObject spawnableParent;

    public void Replace()
    {
        if (usernameField != null && passwordField != null)
        {
            if (usernameField.text == username && passwordField.text == password)
            {
                if (deactivated != null) deactivated.SetActive(false);
                if (activated != null) activated.SetActive(true);
            }
        }
        else
        {
            if (deactivated != null) deactivated.SetActive(false);
            if (activated != null) activated.SetActive(true);
        }       
    }

    public void qrCode(bool respawn)
    {
        if (respawn)
        {
            Instantiate(spawnable, spawnableParent.transform);
        }
        else
        {
            Destroy(GameObject.Find("CrumblingPieces"));
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
