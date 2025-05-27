
using UnityEngine;

public class VectorUtils
{

    public static Vector3 GetRandomRotationAlongZ()
    {
        return new Vector3(0, 0, Random.Range(-360f, 360f));
    }

    public static Vector2 GetRandomForce2D(float minForce, float maxForce)
    {
        int xDirection = Random.Range(0, 2);
        int yDirection = Random.Range(0, 2);
        Vector2 force = new Vector2
        {
            x = Random.Range(minForce, maxForce) * (xDirection == 0 ? -1 : 1),
            y = Random.Range(minForce, maxForce) * (yDirection == 0 ? -1 : 1)
        };

        return force;
    }
}