using UnityEngine;

public class Chaser : Enemy
{
    public Transform? player; // The player to chase

    void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void ChasePlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found.  Cannot chase.");
            return;
        }

        enemyState = EnemyState.Chasing;
        // Implement Chase logic here (e.g., move towards the player)
        Vector3 direction = (player.position - transform.position).normalized;
        Move(direction);
    }

    public void Retreat()
    {
        enemyState = EnemyState.Retreating;
        // Implement Retreat logic here (e.g., move away from the player)
    }
}
