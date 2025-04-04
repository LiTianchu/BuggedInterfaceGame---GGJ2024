using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Banana : MonoBehaviour
{
    public TextMeshProUGUI bananaText;
    public int secretCode = 69;
    public string morseCode = "000111000";
    public float timeInterval = 0.5f;
    public GameObject steamAchievement;
    public GameObject optionsMenu;
    public GameObject graphics;
    public GameObject coinPrefab;
    public AudioClip bananaSound;
    public AudioClip bananaSound2;
    public AudioSource audioSource;
    private int bananaCount = 0;
    private string morse = "";
    private float timeSinceLastPress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastPress += Time.deltaTime;

        if (bananaCount == secretCode)
        {
            bananaText.text = morseCode;
        }
        else {
            bananaText.text = bananaCount.ToString();
        }
    }

    public void AddBanana(int amount)
    {
        bananaCount += amount;

        if (timeSinceLastPress >= timeInterval)
        {
            morse += "1"; 
            audioSource.PlayOneShot(bananaSound);
        }
        else
        {
            morse += "0"; 
            audioSource.PlayOneShot(bananaSound2);
        }
        timeSinceLastPress = 0f;        

        Debug.Log("Morse code: " + morse);
        if (morse.Contains(morseCode))
        {
            steamAchievement.GetComponent<Animator>().SetTrigger("Unlock");
            morse = ""; // Reset morse code after unlocking
        }
    }

    public void ActivateOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void ActivateGraphics()
    {
        graphics.SetActive(true);
        coinPrefab.SetActive(true);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void GetCoin() {
        Debug.Log("Coin collected!");
    }
}
