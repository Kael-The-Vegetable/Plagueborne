using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spector : MonoBehaviour
{
    public float speed;
    public float searchRadius;
    public float angleChangingSpeed;
    [Space]
    public int damage;

    private Transform _target;
    private Rigidbody2D _body;
    private int _enemyLayerID;
    private int _enemyLayer;
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _enemyLayerID = LayerMask.NameToLayer("Enemy");
        _enemyLayer = 1 << _enemyLayerID;
    }
    void FixedUpdate()
    {
        #region Searching
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, searchRadius, _enemyLayer);
        float minDistance = float.MaxValue;
        _target = null;
        for (int i = 0; i < objects.Length; i++)
        {
            float distance = Vector2.Distance(transform.position, objects[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _target = objects[i].transform;
            }
        }
        #endregion
        #region Movement
        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.position - _body.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _body.angularVelocity = -angleChangingSpeed * rotateAmount;
        }
        else
        {
            _body.angularVelocity = 0;
        }
        _body.velocity = transform.up * speed;
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == _enemyLayerID)
        {
            gameObject.SetActive(false);
            if (collider.gameObject.TryGetComponent(out IDamagable damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
