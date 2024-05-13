using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : Actor, IReactive, IDamagable
{
    #region Movement Variables
    private Vector2 _direction;
    private Rigidbody2D _body;
    #endregion
    
    #region Attacks
    [Header("Attacks")]
    public GameObject mainAttack;
    public GameObject secondaryAttack;
    #endregion

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        switch(CurrentState)
        {
            case State.Idle:
            case State.Walk:
                Move();
                break;
        }
    }

    #region Attack Methods
    public void MainAttackControl(InputAction.CallbackContext context)
    {
        if (state != State.Attack)
        { Attack(mainAttack); }
    }
    public void SecondaryAttackControl(InputAction.CallbackContext context)
    {
        if (state != State.Attack)
        { Attack(secondaryAttack); }
    }
    private void Attack(GameObject attack)
    {
        if (!attack.activeInHierarchy)
        {
            CurrentState = State.Attack;
            attack.SetActive(true);
        }
    }
    #endregion

    #region Damage Methods
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        CurrentState = State.Hit;
        if (currentHP <= 0)
        { Die(); }
    }
    public void Die()
    {
        StopAllCoroutines();
        GameState.ChangeScene(0);
    }
    #endregion

    #region Movement Methods
    public void MoveControl(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }
    private void Move()
    {
        if (_direction != Vector2.zero)
        {
            _body.AddForce(_direction * speed * Time.deltaTime);
            CurrentState = State.Walk;
        }
        else
        { CurrentState = State.Idle; }
    }
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

    #region Pause Method
    public void PauseControl(InputAction.CallbackContext context)
    {
        Singleton.Global.State.PauseGame();
    }
    #endregion
}
