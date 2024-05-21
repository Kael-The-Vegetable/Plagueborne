using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private int[] _typeIDs;
    private ObjectPool[] _pools;
    void Awake()
    {
        Transform[] types = InitalizeTypes();
        _typeIDs = new int[types.Length];
        _pools = FindObjectsOfType<ObjectPool>();
        for (int i = 0; i < _pools.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < types.Length && !found; j++)
            {
                if (_pools[i].prefab == types[j])
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
    public ObjectPool GetPeasantPool() => _pools[_typeIDs[0]];
    public ObjectPool GetSlimePool() => _pools[_typeIDs[1]];
}
