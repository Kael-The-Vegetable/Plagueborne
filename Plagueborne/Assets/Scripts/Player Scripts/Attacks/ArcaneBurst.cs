using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBurst : Attack
{
    public float force = 1000;
    public float radius = 2;
    [Space]
    public float flingForce = 0.1f;
    public float flingDuration = 0.25f;

    private IReactive _reactive;
    private int _enemyLayer;
    internal void Start()
    {
        _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
    internal override IEnumerator Initialize(float t)
    {
        yield return new WaitForSeconds(t);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent(out _reactive))
            {
                Vector2 dir = (hitColliders[i].transform.position - transform.position).normalized;
                _reactive.ApplyForce(dir * force);
                _reactive.Fling(flingForce, flingDuration);
            }
            if (hitColliders[i].TryGetComponent(out _damagable))
            {
                _damagable.TakeDamage(damage);
            }
            
        }
        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
