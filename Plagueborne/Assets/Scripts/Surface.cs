using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    public enum SurfaceType
    {
        Slippery,
        Sticky,
        Toxic
    }
    public SurfaceType type;
    public float value;
    private float _inverseValue;

    private void Awake()
    {
        _inverseValue = 1 / value;
    }

    /// <summary>
    /// If colliding with a IReactive, it will use the methods in that interface.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(type)
        {
            case SurfaceType.Slippery:
                if (collision.gameObject.TryGetComponent(out IReactive slipping))
                { 
                    slipping.Slip(_inverseValue, true); 
                }
                break;
            case SurfaceType.Sticky:
                if (collision.gameObject.TryGetComponent(out IReactive sticking))
                {
                    sticking.Stick(value, true); 
                }
                break;
            case SurfaceType.Toxic:
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (type)
        {
            case SurfaceType.Slippery:
                if (collision.gameObject.TryGetComponent(out IReactive slipping))
                { slipping.Slip(value, false); }
                break;
            case SurfaceType.Sticky:
                if (collision.gameObject.TryGetComponent(out IReactive sticking))
                { sticking.Stick(_inverseValue, false); }
                break;
            case SurfaceType.Toxic:
                break;
        }
    }
}
