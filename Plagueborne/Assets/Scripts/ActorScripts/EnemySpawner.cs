using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform target;
    [Space]
    public int numOfEntities;

    public float timeBetweenEntities = 0.1f; // must be greater than 0
    private float _currentTime;
    [Space]
    public int minDistance;
    public int maxDistance;

    public List<ObjectPool> pools;
    public List<float> poolPercentages;

    private int _objectNumber;
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > timeBetweenEntities)
        {
            _currentTime = 0;
            float randNum = Random.value;
            for (int i = 0; i < poolPercentages.Count; i++)
            {
                if (poolPercentages[i] > randNum)
                {// found
                    SpawnEnemies(pools[i]);
                }
                else
                {// not found, remove percentage away
                    randNum -= poolPercentages[i];
                }
            }
        }
        
        
        
        
        //for (int i = 0; i < _objectNumber; i++)
        //{
        //    if (objects[i] != null)
        //    {
        //        if (Vector2.Distance(objects[i].position, target.position) > maxDistance)
        //        { NewPosition(objects[i]); }
        //    }
        //}
    }

    private void SpawnEnemies(ObjectPool pool)
    {
        _objectNumber = Singleton.Global.Objects.ActiveObjects;
        if (_objectNumber < numOfEntities)
        {
            NewObject(pool);
        }
    }

    /// <summary>
    /// Sets the location of a monster at a random range but inside the maximum distance allowed.
    /// </summary>
    /// <param name="transform"></param>
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
