using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Abstract base class for all enemy types, inheriting from Character and implementing IEnemy.
/// Provides basic enemy behaviors and states.
/// </summary>
public abstract class Enemy : Character, IEnemy
{
    // Enum to define possible enemy states
    public enum EnemyState { Idle, Chasing }

    protected EnemyState enemyState;
    protected Transform target;

    /// <summary>
    /// Gets the current enemy state.
    /// </summary>
    public EnemyState GetEnemyState() => enemyState;

    /// <summary>
    /// Destroys the enemy game object.
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    // Abstract methods to be implemented by derived classes
    public abstract void Attack(IPlayer player);
    public abstract int GetScoreValue();

    /// <summary>
    /// Virtual update method with basic chase logic.
    /// </summary>
    protected virtual void Update()
    {
        if (target != null)
        {
            // Basic chase logic
            ClampPosition();
            transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
        }
    }
}