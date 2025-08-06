using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private LayerMask layerToKill;

    void OnTriggerEnter2D(Collider2D other)
    {
        if((1<<other.gameObject.layer & layerToKill) != 0)
        {
            Destroy(other.gameObject);
        }
    }
}
