using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    private Transform[] _types;
    private int[] _typeIDs;
    private ObjectPool[] _pools;
    void Awake()
    {
        _types = InitalizeTypes();
        _typeIDs = new int[_types.Length];
        _pools = FindObjectsOfType<ObjectPool>();
        LinkObjectPools();
        SceneManager.activeSceneChanged += SceneChanged; // adding the SceneChanged method to the event.
    }
    private void SceneChanged(Scene old, Scene next)
    {
        if (next.buildIndex != 0)
        {
            GameObject[] newObjects = next.GetRootGameObjects();
            List<ObjectPool> newPools = new List<ObjectPool>();
            for (int i = 0; i < newObjects.Length; i++)
            {
                if (newObjects[i].TryGetComponent(out ObjectPool pool))
                { newPools.Add(pool); }
            }
            _pools = newPools.ToArray();
            LinkObjectPools();
        }
    }
    private void LinkObjectPools()
    {
        for (int i = 0; i < _pools.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < _types.Length && !found; j++)
            {
                if (_pools[i].prefab == _types[j])
                {
                    found = true;
                    _typeIDs[j] = i;
                }
            }
        }
    }
    private Transform[] InitalizeTypes()
    {
        Transform[] types = new Transform[2]; // change number when make more enemies
        types[0] = Resources.Load<Transform>("Prefabs/Enemies/Peasant");
        types[1] = Resources.Load<Transform>("Prefabs/Enemies/Slime");
        return types;
    }

    /// <summary>
    /// Gets all the active objects which are in the types allocated by ObjectManager.
    /// </summary>
    public int ActiveObjects
    {
        get
        {
            int numObj = 0;
            for (int i = 0; i < _typeIDs.Length; i++)
            {
                numObj += _pools[_typeIDs[i]].NumOfObjects;
            }
            return numObj;
        }
    }

    /// <summary>
    /// Gets all the Transforms of active objects in the types allocated by ObjectManager.
    /// </summary>
    public Transform[] ActiveObjectTransforms
    {
        get
        {
            List<Transform> transformList = new List<Transform>();
            for (int i = 0; i < _typeIDs.Length; i++)
            {
                transformList.AddRange(_pools[_typeIDs[i]].ActiveTransforms);
            }
            return transformList.ToArray();
        }
    }

    /// <summary>
    /// Gets the pool for Peasants
    /// </summary>
    /// <returns></returns>
    public ObjectPool GetPeasantPool() => _pools[_typeIDs[0]];
    
    /// <summary>
    /// Gets the pool for Slimes
    /// </summary>
    /// <returns></returns>
    public ObjectPool GetSlimePool() => _pools[_typeIDs[1]];
}
