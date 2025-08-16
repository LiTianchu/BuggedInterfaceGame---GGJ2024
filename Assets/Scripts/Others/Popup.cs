using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] public GameObject popupPanel; // The Panel
    [SerializeField] public float time = Mathf.Infinity; // Timer to show the popup
    [SerializeField] private bool oneTime = true; // Show the popup only once

    private float timer;
    private bool shown = false;

    void Start()
    {
        timer = time;
    }

    void Update()
    {
        if (time < 0f)
        { 
            return; // If time is negative, do not show the popup by timer
        }

        if ((!oneTime || !shown) && !popupPanel.activeInHierarchy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ShowPopup();
                timer = time;
                shown = true;
            }
        }
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}