using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : Attack
{
    [Tooltip("The lower the number the more of the delay will be into after the teleport.")]
    public float timeRatio;
    [Space]
    public float distanceFromPlayer;
    [Space]
    public float force = 1000;
    public float blastRadius = 1;
    [Space]
    public float flingForce = 0.1f;
    public float flingDuration = 0.25f;

    private IReactive _reactive;
    private int _enemyLayer;
    private PlayerController _player;

    internal void Awake()
    {
        _player = transform.parent.GetComponent<PlayerController>();
        _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
    internal override void OnDisable()
    {
    }
    internal override IEnumerator Initialize(float time)
    {
        yield return new WaitForSeconds(time * timeRatio);
        transform.localPosition = _player.LookingDir * distanceFromPlayer;
        transform.parent.transform.position += transform.localPosition;
        transform.localPosition = Vector2.zero;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius, _enemyLayer);
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
        transform.parent.GetComponent<Actor>().CurrentState = Actor.State.Idle;
        yield return new WaitForSeconds(time - time * timeRatio);
        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.parent.position, transform.position);
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
