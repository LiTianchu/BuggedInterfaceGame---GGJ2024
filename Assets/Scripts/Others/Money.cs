using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Money : MonoBehaviour
{
    int amount = 0;
    int stock = 0;
    int price = 240;

    [SerializeField] TextMeshProUGUI amt;
    [SerializeField] TextMeshProUGUI stk;
    [SerializeField] GameObject win;
    
    public TextMeshProUGUI success;
    public GameObject qr;

    float timer = 0f;

    public void CheckAmount()
    {
        if (price / stock == amount)
        {
            win.SetActive(true);
            StartCoroutine(EndGame());
        }
        else if (success != null) success.text = "Incorrect amount of MOGCOIN";
    }

    public void IncreaseAmount(bool increase)
    {
        if (increase) amount = Mathf.Clamp(amount + 1, 0, 999);
        else amount = Mathf.Clamp(amount - 1, 0, 999);
    }

    void ChangeStock()
    {
        stock = Random.Range(2, 12);
        while (stock == 7 || stock == 9 || stock == 11) stock = Random.Range(2, 12);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("Credits");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ChangeStock();
            timer = 60f;
        }

        amt.text = amount.ToString();
        stk.text = "$" + stock.ToString();

        if (success != null && !success.gameObject.active) success.text = "";
    }
}
