using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static AnimationUtils;

public class Change : MonoBehaviour
{
    [Header("Authentication Settings(Optional)")]
    [SerializeField] string username;
    [SerializeField] string password;
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;

    [Header("GameObjects to Activate/Deactivate upon changing displayed panel")]
    [SerializeField] GameObject deactivated;
    [SerializeField] GameObject activated;

    public void Replace()
    {
        bool authenticated = true;

        // Check authentication if fields are provided
        if (usernameField != null && passwordField != null)
        {
            authenticated = usernameField.text == username && passwordField.text == password;
        }

        if (authenticated)
        {
            // Immediate replacement without transition
            if (deactivated != null)
            {
                UIManager.Instance.HideUI(deactivated);
            }

            if (activated != null)
            {
                UIManager.Instance.ShowUI(activated);
            }
        }
    }
}
