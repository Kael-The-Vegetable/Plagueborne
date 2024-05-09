using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHub : MonoBehaviour
{
    private AudioSource[] _sources;
    void Awake()
    {
        _sources = GetComponents<AudioSource>();
    }
    public void PlayBlip() => _sources[0].Play();
}
