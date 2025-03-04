using UnityEngine;
using GamePlayDLL;
// This script defines an abstract base class for all enemy types, inheriting from Character and implementing IEnemy. It includes basic enemy behaviors and states.
public abstract class Enemy : Character, IEnemy
{
    // Enum to define possible enemy states
    public enum EnemyState { Idle, Chasing }
    protected EnemyState enemyState;
    protected Transform target;

    // Method to get the current enemy state
    public EnemyState GetEnemyState() => enemyState;

    // Method to destroy the enemy game object
    public void Destroy()
    {
        Destroy(gameObject);
    }

    // Abstract methods to be implemented by derived classes
    public abstract void Attack(IPlayer player);
    public abstract int GetScoreValue();

    // Virtual update method with basic chase logic
    protected virtual void Update()
    {
        if (target != null)
        {
            // Basic chase logic
            transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
        }
    }
}