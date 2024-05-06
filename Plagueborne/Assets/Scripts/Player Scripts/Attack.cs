using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected IDamagable _damagable;
    public int damage;
    [Space]
    public float time;
    internal virtual void OnEnable()
    {
        StartCoroutine(Initialize(time));
    }
    internal virtual void OnDisable()
    {
        transform.parent.GetComponent<Actor>().CurrentState = Actor.State.Idle;
    }
    internal abstract IEnumerator Initialize(float time);
}
