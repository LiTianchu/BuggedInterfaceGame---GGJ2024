using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popupPanel; // The Panel
    private float timer = 60f; // 60 seconds timer

    void Update()
    {
        Debug.Log("Timer: " + timer + ", Popup Active: " + popupPanel.activeInHierarchy);

        if (!popupPanel.activeInHierarchy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Debug.Log("Showing popup");
                ShowPopup();
                timer = 60f;
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