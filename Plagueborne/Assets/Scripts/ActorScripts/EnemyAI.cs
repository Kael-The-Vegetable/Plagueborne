using Pathfinding;
using System.Collections;
using UnityEngine;

public abstract class EnemyAI : Actor, IDamagable
{
    protected Transform _target;
    protected Seeker _seeker;
    protected Rigidbody2D _body;
    [Space]
    public Path path;
    public float nextWaypointDistance = 2f;
    protected int _currentWaypoint = 0;

    public float repathRate = 0.25f;
    [Space]
    public float attackDistance;
    public float damage;
    protected Coroutine _hitCoroutine;
    protected Coroutine _attackCoroutine;

    protected int _enemyLayer;

    #region Reset Variables
    protected float _originalDrag;
    protected float _originalSpeed;
    #endregion

    internal virtual void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _seeker = GetComponent<Seeker>();
        _body = GetComponent<Rigidbody2D>();
        _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        _originalDrag = _body.drag;
        _originalSpeed = speed;

        InvokeRepeating("UpdatePath", 0, repathRate);
    }
    internal virtual void OnEnable()
    {
        CurrentState = State.Idle;
        currentHP = MaxHitPoints;
        speed = _originalSpeed;
        _body.drag = _originalDrag;
    }
    internal abstract void FixedUpdate();
    internal virtual void UpdatePath()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _target.position - transform.position, Vector2.Distance(transform.position, _target.position) + 1, ~_enemyLayer);
        if (hit.transform == _target && path != null)
        {
            path.vectorPath.Clear();
            path.vectorPath.Add(_target.position);
            _currentWaypoint = 0;
        }
        else if (_seeker.IsDone())
        {
            _seeker.StartPath(transform.position, _target.position, OnPathComplete); 
        }    
    }
    internal virtual void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            _currentWaypoint = 0;
        }
    }
    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        { Die(); }
        else if (damage >= HitThreshold)
        {
            if (_attackCoroutine != null)
            { StopCoroutine(_attackCoroutine); }
            if (_hitCoroutine != null)
            { StopCoroutine(_hitCoroutine); }
            
            _hitCoroutine = StartCoroutine(GameState.DelayedVarChange(result => CurrentState = result, 0.5f, State.Hit, State.Idle));
        }
    }
    public virtual void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        CurrentState = State.Die;
        Singleton.Global.State.Kills++;
    }

    private void OnDrawGizmos()
    {
        if (path != null && path.vectorPath.Count == 1)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, path.vectorPath[0]);
        }
    }
}
