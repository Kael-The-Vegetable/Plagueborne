using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// BROKEN! FIX LATER!!!
public class SlimeEnemy : EnemyAI
{
    [Space]
    public float walkTimer = 0.5f;
    private float _currentTime;

    public int splitAmount;
    public int splitIntoNumber;

    private Collider2D _targetCollider;
    private Collider2D _collider;

    private Transform[] _childSlimes;
    
    internal override void Awake()
    {
        base.Awake();

        _targetCollider = _target.GetComponent<Collider2D>();
        _collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(_targetCollider, _collider);

        #region SlimeChildren Setup
        if (splitAmount > 0)
        {
            float inverseSplitNum = 1f / splitIntoNumber;
            _childSlimes = new Transform[splitIntoNumber];
            splitAmount--;
            speed *= inverseSplitNum;
            damage *= inverseSplitNum;
            MaxHitPoints *= inverseSplitNum;
            for (int i = 0; i < splitIntoNumber; i++)
            {
                _childSlimes[i] = Instantiate(transform, Vector2.zero, Quaternion.identity, transform.parent);
                Transform slime = _childSlimes[i];
                slime.localScale = transform.localScale * inverseSplitNum;
                slime.gameObject.SetActive(false);
                Rigidbody2D slimeBody = slime.GetComponent<Rigidbody2D>();
                slimeBody.mass = GetComponent<Rigidbody2D>().mass * inverseSplitNum;
            }
            splitAmount++;
            speed *= splitIntoNumber;
            damage *= splitIntoNumber;
            MaxHitPoints *= splitIntoNumber;
        }
        #endregion
    }
    internal override void FixedUpdate()
    {
        if (path == null || _currentWaypoint >= path.vectorPath.Count)
        { return; }

        switch (CurrentState)
        {
            case State.Idle:
            case State.Walk:
                _currentTime += Time.deltaTime;
                // walking force
                if (_currentTime >= walkTimer)
                {
                    _currentTime = 0;
                    Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
                    _body.AddForce(dir * speed * Time.deltaTime);
                }

                // seeing if we are walking or just idle
                if (_body.velocity.magnitude > 0)
                { CurrentState = State.Walk; }
                else
                { CurrentState = State.Idle; }
                if (Physics2D.Distance(_collider, _targetCollider).isOverlapped)
                {// colliding with target
                    // make target take damage;
                    Debug.Log("HIT");
                }
                break;
        }

        float distance = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);
        if (distance < nextWaypointDistance && _currentWaypoint + 1 < path.vectorPath.Count)
        { _currentWaypoint++; }
    }
    public override void Die()
    {
        if (splitAmount > 0)
        { Split(); }
        base.Die();
    }
    internal void Split()
    {
        for (int i = 0; i < splitIntoNumber; i++)
        {
            _childSlimes[i].gameObject.SetActive(true);
            _childSlimes[i].position = (Vector2)transform.position + Random.insideUnitCircle;
        }
    }
}
