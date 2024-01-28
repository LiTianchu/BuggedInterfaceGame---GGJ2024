using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popupPanel; // The Panel
    private float timer = 90f; // 60 seconds timer

    void Update()
    {
        if (!popupPanel.activeInHierarchy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ShowPopup();
                timer = 90f;
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