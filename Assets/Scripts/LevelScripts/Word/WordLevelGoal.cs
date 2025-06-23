using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLevelGoal : MonoBehaviour
{
    [SerializeField] private LayerMask wordPlayerLayer;
    [SerializeField] private float moveSpeed = 2f;

    private bool _isCollected = false;
    private float _timeSinceCollected = 0f;
    public event System.Action OnGoalCollected;
    void Update()
    {
        if (_isCollected)
        {
            _timeSinceCollected += Time.deltaTime;
            transform.position += Vector3.up * Time.deltaTime * moveSpeed; // Move the goal up after collection
            if(_timeSinceCollected > 10f)
            {
                Destroy(gameObject); // Destroy the goal after 10 seconds
            }
        }    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isCollected && ((1 << collision.gameObject.layer) & wordPlayerLayer) != 0)
        { // check if the collision is with the doodle
            OnGoalCollected?.Invoke();
            _isCollected = true;
        }
    }
    
}
