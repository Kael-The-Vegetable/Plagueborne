using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [field: SerializeField] public float MaxHitPoints { get; set; } = 10;
    internal float currentHP;
    [field: SerializeField] public float HitThreshold { get; set; } = 5;
    [SerializeField] internal float speed = 500;

    #region State Variables
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Hit,
        Die
    }
    internal State state;
    public State CurrentState
    {
        get => state;
        internal set
        {
            if (state != value)
            { state = value; }
        }
    }
    #endregion
}
