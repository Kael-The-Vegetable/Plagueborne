using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform target;
    [Space]
    public int numOfEntities;

    [Tooltip("0 for instantanious spawning")]
    public float timeBetweenEntities; // 0 for instantanious
    [Space]
    public int minDistance;
    public int maxDistance;

    private int _objectNumber;
    private void Start()
    {
        if (timeBetweenEntities > 0)
        { InvokeRepeating("SpawnEnemies", 0, timeBetweenEntities); }
        else
        {
            for (int i = 0; i < numOfEntities; i++)
            { NewObject(Singleton.Global.Objects.GetPeasantPool()); }
        }
    }
    void Update()
    {
        ObjectPool peasantPool = Singleton.Global.Objects.GetSlimePool();
        if (timeBetweenEntities == 0)
        {
            for (int i = _objectNumber; i < numOfEntities; i++)
            { NewObject(peasantPool); }
        }
        
        
        Transform[] objects = peasantPool.GetObjectTransforms(_objectNumber);
        Debug.Log($"Number Recorded: {_objectNumber} | Number Actual: {peasantPool.NumOfObjects}");
        for (int i = 0; i < _objectNumber; i++)
        {
            if (objects[i] != null)
            {
                if (Vector2.Distance(objects[i].position, target.position) > maxDistance)
                { NewPosition(objects[i]); }
            }
        }
    }

    private void SpawnEnemies()
    {
        _objectNumber = Singleton.Global.Objects.ActiveObjects;
        if (_objectNumber < numOfEntities)
        {
            NewObject(Singleton.Global.Objects.GetSlimePool());
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
