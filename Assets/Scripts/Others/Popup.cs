using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popupPanel; // The Panel
    public float time = Mathf.Infinity; // Timer to show the popup
    private float timer;

    void Start() {
        timer = time;
    }

    void Update()
    {
        if (!popupPanel.activeInHierarchy)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ShowPopup();
                timer = time;
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