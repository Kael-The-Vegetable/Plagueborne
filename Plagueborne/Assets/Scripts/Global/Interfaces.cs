using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactive
{
    /// <summary>
    /// This is the opposite of the Stick method where drag is reduced instead of increased.
    /// </summary>
    /// <param name="multiplier"></param>
    /// <param name="isSlipping"></param>
    public void Slip(float multiplier, bool isSlipping);
    /// <summary>
    /// This is the opposite of the Slip method where drag is increased instead of reduced.
    /// </summary>
    /// <param name="multiplier"></param>
    /// <param name="isSticking"></param>
    public void Stick(float multiplier, bool isSticking);
    /// <summary>
    /// This method should be used to apply an instantanious force to the rigidbody attached to this object.
    /// </summary>
    /// <param name="force"></param>
    public void ApplyForce(Vector2 force);
    /// <summary>
    /// multiplier should be between 0 and 1. Any value above 1 will count to the increased friction of the ground.<br />
    /// duration should be a non-negative number. <br /><br />
    /// This method is to help explosions feel more impactful.
    /// </summary>
    /// <param name="multiplier"></param>
    /// <param name="duration"></param>
    public void Fling(float multiplier, float duration);
}
public interface IDamagable
{
    public int MaxHitPoints { get; set; }
    public void TakeDamage(int damage);
    public void Die();
}
