using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool pool;
    public Transform target;
    [Space]
    public int numOfEntities;
    public int minDistance;
    public int maxDistance;

    void Start()
    {
        for (int i = 0; i < numOfEntities; i++)
        { NewObject(pool); }
    }
    void Update()
    {
        int objNum = pool.NumOfObjects;
        for (int i = objNum; i < numOfEntities; i++)
        { NewObject(pool); }
        Transform[] objects = pool.GetObjectTransforms(numOfEntities);
        for (int i = 0; i < numOfEntities; i++)
        {
            if (Vector2.Distance(objects[i].position, target.position) > maxDistance)
            { NewPosition(objects[i]); }
        }
    }
    public void NewPosition(Transform transform)
    {
        Vector2 pos = Random.insideUnitCircle.normalized;
        transform.position = (Vector2)target.position + pos * Random.Range(minDistance, maxDistance - 1);
    }

    /// <summary>
    /// Makes a new Object using the object pool at a random location using the method NewPosition().
    /// </summary>
    /// <returns>true on a new object being found,<br /> false on no object being found available</returns>
    public bool NewObject(ObjectPool pool)
    {
        bool returned = false;
        Transform newObject = pool.Object;
        if (newObject != null)
        { 
            NewPosition(newObject); 
            returned = true;
        }
        return returned;
    }
}
