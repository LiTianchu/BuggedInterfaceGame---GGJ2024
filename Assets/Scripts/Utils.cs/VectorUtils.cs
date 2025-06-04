
using System.Collections.Generic;
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


    public static Vector2 GetRandomPointOutsideBox(Vector2 lowerLeft, Vector2 upperRight, float minOffset, float maxOffset)
    {
        float randNum = Random.Range(0, 1.0f);
        float randomOffset = Random.Range(minOffset, maxOffset);
        float x;
        float y;
        if (randNum < 0.25f)
        { // top
            x = Random.Range(lowerLeft.x, upperRight.x);
            y = upperRight.y + randomOffset;
        }
        else if (randNum < 0.5f)
        { // bottom
            x = Random.Range(lowerLeft.x, upperRight.x);
            y = lowerLeft.y - randomOffset;
        }
        else if (randNum < 0.75f)
        { // left
            y = Random.Range(lowerLeft.y, upperRight.y);
            x = lowerLeft.x - randomOffset;
        }
        else
        { // right
            y = Random.Range(lowerLeft.y, upperRight.y);
            x = upperRight.x + randomOffset;
        }

        return new Vector2(x, y);
    }

       /// <summary>
    /// Generates a list of positions evenly distributed around a ring
    /// </summary>
    /// <param name="centerPosition">Center of the ring</param>
    /// <param name="radius">Radius of the ring</param>
    /// <param name="pointDistance">Distance between points on the ring</param>
    /// <returns>List of positions around the ring</returns>
    public static List<Vector3> GetRingSamplePositions(Vector3 centerPosition, float radius, float pointDistance)
    {
        List<Vector3> positions = new();

        // Calculate how many points can fit around the ring based on distance between points
        float circumference = 2 * Mathf.PI * radius;
        int numberOfPoints = Mathf.FloorToInt(circumference / pointDistance);

        // Calculate the angle between each point
        float angleStep = 360f / numberOfPoints;

        for (int i = 0; i < numberOfPoints; i++)
        {
            // Calculate the angle for this point
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Calculate the position on the ring
            Vector3 position = new Vector3(
                centerPosition.x + radius * Mathf.Cos(angle),
                centerPosition.y + radius * Mathf.Sin(angle),
                centerPosition.z
            );

            positions.Add(position);
        }

        return positions;
    }
}