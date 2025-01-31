using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Change : MonoBehaviour
{
    [SerializeField] string username;
    [SerializeField] string password;
    [SerializeField] TMP_InputField usernameField; 
    [SerializeField] TMP_InputField passwordField;
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
            //GameObject qrtemp = FindObjectOfType<CrumblingPieces>().gameObject;
            //if (qrtemp != null) Destroy(qrtemp);
            GameObject qr = Instantiate(spawnable, spawnableParent.transform);
            qr.SetActive(true);
            FindObjectOfType<Money>().qr = qr;
            FindObjectOfType<Money>().success = qr.transform.Find("Notification").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Destroy(FindObjectOfType<Money>().qr);
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
