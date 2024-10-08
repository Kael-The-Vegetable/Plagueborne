using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropshadow : MonoBehaviour
{
    public Sprite spriteToUse;
    public Material material;
    [Space]
    public Vector2 offset;
    public Vector2 scale = Vector2.one;
    public Transform targetToFollow;

    private GameObject _shadow;
    void Start()
    {
        _shadow = new GameObject("Shadow");
        _shadow.transform.parent = targetToFollow;
        _shadow.transform.localScale = scale;
        _shadow.transform.localRotation = Quaternion.identity;
        _shadow.transform.localPosition = offset;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer shadowRenderer = _shadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = spriteToUse;
        shadowRenderer.material = material;

        shadowRenderer.sortingLayerName = renderer.sortingLayerName;
        shadowRenderer.sortingOrder = renderer.sortingOrder - 1;
    }
}
