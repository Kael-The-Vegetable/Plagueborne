using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulExpulsion : Attack
{
    public float attackSpeed = 0.25f;
    [Space]
    public float spectorSpeed = 1;
    public float spectorSearchRadius = 2;
    public float spectorRotateRate = 60;

    private ObjectPool _pool;
    internal void Awake()
    {
        _pool = transform.parent.GetComponent<ObjectPool>();
    }
    internal override IEnumerator Initialize(float time)
    {
        float times = time / attackSpeed;
        float remainder = times % 1 * attackSpeed;
        for (int i = 0; i < times; i++)
        {
            SpawnSpector();
            yield return new WaitForSeconds(attackSpeed);
        }
        yield return new WaitForSeconds(remainder);
        gameObject.SetActive(false);
    }
    private void SpawnSpector()
    {
        Transform obj = _pool.Object;
        if (obj != null )
        {
            obj.Rotate(new Vector3(0, 0, Random.Range(0, 359)));
            obj.position = transform.position;
            Spector spector = obj.GetComponent<Spector>();
            spector.speed = spectorSpeed;
            spector.searchRadius = spectorSearchRadius;
            spector.angleChangingSpeed = spectorRotateRate;
            spector.damage = damage;
        }
    }
}
