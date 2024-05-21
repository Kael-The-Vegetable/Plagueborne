using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyAI, IReactive
{
    public float attackTime = 2;
    public float attackRatio = 0.33f;
    [Space]
    public float lungeForce = 1.5f;
    internal override void FixedUpdate()
    {
        if (path == null || _currentWaypoint >= path.vectorPath.Count)
        { return; }

        switch (CurrentState)
        {
            case State.Idle:
            case State.Walk:
                
                // walking force
                Vector2 dir = (path.vectorPath[_currentWaypoint] - transform.position).normalized;
                _body.AddForce(dir * speed * Time.deltaTime);
                
                // seeing if we are walking or just idle
                if (_body.velocity.magnitude > 0)
                { CurrentState = State.Walk; }
                else
                { CurrentState = State.Idle; } 

                // if we are in attack range
                if (Vector2.Distance(transform.position, path.vectorPath[path.vectorPath.Count - 1])
                    <= attackDistance)
                {
                    // if there is an attack already in effect, remove it
                    if (_attackCoroutine != null)
                    { StopCoroutine(_attackCoroutine); }

                    // start an attack
                    _attackCoroutine = StartCoroutine(GameState.DelayedVarChange(
                        result => CurrentState = result, attackTime, State.Attack, State.Idle));
                    StartCoroutine(Lunge(attackTime, attackRatio));
                }
                break;
        }

        float distance = Vector2.Distance(transform.position, path.vectorPath[_currentWaypoint]);
        if (distance < nextWaypointDistance && _currentWaypoint + 1 < path.vectorPath.Count)
        { _currentWaypoint++; }
    }

    /// <summary>
    /// LungRatio refers to the ratio of moving Back : moving Forward.
    /// </summary>
    internal IEnumerator Lunge(float attackDuration, float lungRatio)
    {
        Vector2 dir = (transform.position - path.vectorPath[path.vectorPath.Count - 1]).normalized;
        _body.AddForce(dir * lungeForce  * speed * 0.25f);
        yield return new WaitForSeconds(attackDuration * lungRatio);
        _body.AddForce(-dir * lungeForce * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {// REWORK SO THAT COLLISION IS A ENABLED COLLISION SHAPE.
        if (collision.collider.CompareTag("Player") && CurrentState == State.Attack && _body.velocity.magnitude > lungeForce)
        {
            Debug.Log("HIT");
        }
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
