using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Boolet
{
    public float speed = 5f; // Speed at which the card moves downwards

    // Update is called once per frame
    protected override void Update()
    {
        if(stickman.IsGameOver)
        {
            return; // Skip update if the game is over
        }
        
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (IsCursorColliding())
        {
            if (name.Contains("Heart Card"))
            {
                stickman.ReduceHealth(1);
                Destroy(gameObject);
            }
            else if (IsCursorColliding() && harmful && !stickman.IsImmune())
            {
                GameOver();

            }
        }
    }
}
