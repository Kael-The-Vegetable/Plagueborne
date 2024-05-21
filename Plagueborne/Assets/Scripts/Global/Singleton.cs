using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{   
    public static Singleton Global { get; private set; }
    public SoundHub Sounds { get; private set; }
    public GameState State { get; private set; }
    public ObjectManager Objects { get; private set; }
    private void Awake()
    {
        if (Global != null && Global != this)
        { Destroy(gameObject); }
        else
        { 
            Global = this;
            DontDestroyOnLoad(gameObject);
        }

        Sounds = GetComponentInChildren<SoundHub>();
        State = GetComponentInChildren<GameState>();
        Objects = GetComponentInChildren<ObjectManager>();
    }
}
