using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HorizontalFlip : MonoBehaviour
{
    public Rigidbody2D _body;
    public SpriteRenderer _renderer;
    public float sensitivity = 0.01f;
    void Update()
    {
        if (_body.velocity.x >= sensitivity)
        { _renderer.flipX = true; }
        else if (_body.velocity.x <= -sensitivity)
        { _renderer.flipX = false; }
    }
}
