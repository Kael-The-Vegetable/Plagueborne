using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool pool;
    [Header("Wave1")]
    public int numOfEntities;
    public int minDistance;
    public int maxDistance;

    void Start()
    {
        Transform newObject;
        for (int i = 0; i < numOfEntities; i++)
        {
            newObject = pool.Object;
            if ( newObject != null )
            {
                float j = (float)i / numOfEntities;
                // get the angle for this step (in radians, not degrees)
                float angle = j * Mathf.PI * 2;
                // the X & Y position for this angle are calculated using Sin & Cos
                float x = Mathf.Sin(angle) * Random.Range(minDistance, maxDistance);
                float y = Mathf.Cos(angle) * Random.Range(minDistance, maxDistance);
                newObject.position = new Vector2(x, y); 
            }
        }
        
    }
}
