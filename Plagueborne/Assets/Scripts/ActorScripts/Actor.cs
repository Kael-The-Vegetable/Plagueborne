using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [field: SerializeField] public int MaxHitPoints { get; set; } = 10;
    internal int currentHP;
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
        internal set => state = value;
    }
    #endregion
}
