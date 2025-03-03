using UnityEngine;

public enum EnemyState { Patrolling, Chasing, Attacking, Retreating }

public class Enemy : Character
{
    public EnemyState enemyState = EnemyState.Patrolling;
    public Transform? target; // The target to chase (e.g., the Player)

    public virtual EnemyState GetEnemyState()
    {
        return enemyState;
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the other collider belongs to the player
        if (other.gameObject.GetComponent<Player>() != null)
        {
            // Access the GameManager and remove a life
            GameManager.Instance?.RemoveLife(); // Use null-conditional operator
            Destroy(gameObject);
        }
    }
    // Example of a method to handle taking damage
    public virtual void Destroy()
    {
        Debug.Log("Enemy Destroyed!");
        Destroy(gameObject);
    }
}
