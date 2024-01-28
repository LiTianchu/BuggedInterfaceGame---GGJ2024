using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    int amount = 0;
    int stock = 0;
    int price = 240;

    [SerializeField] TextMeshProUGUI amt;
    [SerializeField] TextMeshProUGUI stk;

    float timer = 0f;

    public void CheckAmount()
    {
        if (price / stock == amount) Debug.Log("Yes");
        else Debug.Log("no");
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
        stk.text = stock.ToString();
    }
}
