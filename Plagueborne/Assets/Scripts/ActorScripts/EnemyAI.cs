using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAI : Actor, IReactive, IDamagable
{
    private Transform _target;
    private Seeker _seeker;
    private Rigidbody2D _body;
    
    public Path path;
    public float nextWaypointDistance = 2f;
    private int _currentWaypoint = 0;

    public float repathRate = 0.25f;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _seeker = GetComponent<Seeker>();
        _body = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0, repathRate);
    }
    private void OnEnable()
    {
        CurrentState = State.Idle;
        currentHP = MaxHitPoints;
    }
    private void FixedUpdate()
    {
        if (path == null || _currentWaypoint >= path.vectorPath.Count)
        { return; }
        Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
        _body.AddForce(dir * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);
        if (distance < nextWaypointDistance && _currentWaypoint + 1 < path.vectorPath.Count)
        { _currentWaypoint++; }
    }
    private void UpdatePath()
    {
        if (_seeker.IsDone())
        { _seeker.StartPath(transform.position, _target.position, OnPathComplete); }    
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            _currentWaypoint = 0;
        }
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        CurrentState = State.Hit;
        if (currentHP <= 0)
        { Die(); }
    }
    public void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        CurrentState = State.Die;
    }

    #region IReactive Methods
    public void Slip(float multiplier, bool isSlipping)
    {
        if (isSlipping)
        { speed *= multiplier * 2; }
        else
        { speed *= multiplier * 0.5f; }

        _body.drag *= multiplier;
    }
    public void Stick(float multiplier, bool isSticking) => _body.drag *= multiplier;
    public void ApplyForce(Vector2 force) => _body.AddForce(force);
    public void Fling(float multiplier, float duration)
        => StartCoroutine(Flinger(multiplier, duration));
    public IEnumerator Flinger(float multiplier, float duration)
    {
        Slip(multiplier, true);
        yield return new WaitForSeconds(duration);
        Slip(1 / multiplier, false);
    }
    #endregion
}
