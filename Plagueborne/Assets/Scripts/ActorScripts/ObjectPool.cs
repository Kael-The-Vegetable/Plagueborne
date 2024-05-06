using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Transform prefab;
    public int numberOfPrefabs;
    private Transform[] objects = new Transform[0];
    void Awake()
    {
        objects = InitializeArray(objects, numberOfPrefabs, prefab);
    }
    public Transform Object
    {
        get
        {
            Transform returnTransform = null;
            int c = 0;
            while (c < objects.Length && returnTransform == null)
            {
                if (!objects[c].gameObject.activeInHierarchy)
                {
                    returnTransform = objects[c];
                    objects[c].gameObject.SetActive(true);
                }
                c++;
            }
            return returnTransform;
        }
    }
    public int NumOfObjects
    {
        get
        {
            int c = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject.activeInHierarchy)
                { c++; }
            }
            return c;
        }
    }
    public Transform[] GetObjectTransforms(int length)
    {
        Transform[] transforms = new Transform[length];
        int c = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.activeInHierarchy && c < length)
            { transforms[c++] = objects[i]; } // ++ after the variable will return then add one.
        }
        return transforms;
    }
    public Transform[] InitializeArray(Transform[] array, int length, Transform prefab)
    {
        if (array.Length == 0)
        {
            array = new Transform[length];
            for (int c = 0; c < length; c++)
            {
                array[c] = Instantiate(prefab);
                array[c].gameObject.SetActive(false);
            }
        }
        return array;
    }
}
