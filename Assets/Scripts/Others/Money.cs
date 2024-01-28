using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    int amount = 0;
    int stock = 0;
    int price = 232;

    float timer = 60f;

    public void CheckAmount()
    {
        if (price / stock == amount) Debug.Log("Yes");
        else Debug.Log("no");
    }

    public void IncreaseAmount(bool increase)
    {
        if (increase) amount += 1;
        else amount -= 1;
    }

    void ChangeStock()
    {
        stock = Random.Range(1, 20);
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
    }
}
