using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Abstract base class for all enemy types, inheriting from Character and implementing IEnemy.
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
    /// Destroys the enemy game object will use to kill enemies
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    
    public abstract void Attack(IPlayer player);
    public abstract int GetScoreValue();

    /// <summary>
    /// Virtual update method with basic chase logic
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