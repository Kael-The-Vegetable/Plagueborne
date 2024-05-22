using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulExpulsion : Attack
{
    public float attackSpeed = 0.25f;
    public int numSpectorsPerAttack = 2;
    [Space]
    public int spectorTimeAlive = 10;
    public int spectorNumberOfHits = 4;
    public float spectorSpeed = 1;
    public float spectorSearchRadius = 2;
    public float spectorRotateRate = 60;

    public ObjectPool pool;
    internal override void OnDisable()
    {
    }
    internal override IEnumerator Initialize(float time)
    {
        float times = time / attackSpeed;
        float remainder = times % 1 * attackSpeed;
        yield return new WaitForSeconds(0.5f);
        transform.parent.GetComponent<Actor>().CurrentState = Actor.State.Idle;
        for (int i = 0; i < times; i++)
        {
            for (int j = 0; j < numSpectorsPerAttack; j++) 
            { SpawnSpector(); }
            yield return new WaitForSeconds(attackSpeed);
        }
        yield return new WaitForSeconds(remainder);
        gameObject.SetActive(false);
    }
    private void SpawnSpector()
    {
        Transform obj = pool.Object;
        if (obj != null )
        {
            obj.Rotate(new Vector3(0, 0, Random.Range(0, 359)));
            obj.position = transform.position;
            Spector spector = obj.GetComponent<Spector>();
            spector.timeAlive = spectorTimeAlive;
            spector.numberOfHits = spectorNumberOfHits;
            spector.speed = spectorSpeed;
            spector.searchRadius = spectorSearchRadius;
            spector.angleChangingSpeed = spectorRotateRate;
            spector.damage = damage;
        }
    }
}
