using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Represents a chaser enemy that pursues the player within a certain range.
/// This enemy changes color based on its state and deals damage on collision.
/// </summary>
public class Chaser : Enemy
{
    [SerializeField] private float chaseRange = 15f; // Distance within which the enemy chases the player
    [SerializeField] private float idleRange = 20f; // Distance beyond which the enemy becomes idle
    [SerializeField] private float attackRange = 2f; // Distance within which the enemy can attack
    [SerializeField] private int collisionDamage = 10; // Damage dealt on collision with player
    [SerializeField] private Color normalColor = Color.red; // Color when chasing
    [SerializeField] private Color idleColor = Color.grey; // Color when idle

    private Renderer enemyRenderer; // Reference to the enemy's renderer component
    
    private void Start()
    {
        // Find and set the player as the target
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Get the renderer component for color changing
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = normalColor;
        }
    }

    /// <summary>
    /// Handles the chasing behavior of the enemy.
    /// </summary>
    public void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        
        if (distanceToPlayer <= chaseRange)
        {
            enemyState = EnemyState.Chasing;
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = normalColor;
            }
            // Move toward the player
            transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
        }
        else if (distanceToPlayer > idleRange)
        {
            enemyState = EnemyState.Idle;
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = idleColor;
            }
            // Don't move when idle (beyond 20 units)
        }
    }

    /// <summary>
    /// Moves the enemy away from the player.
    /// </summary>
    public void Retreat()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, -movementSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Attacks the player if within range.
    /// </summary>
    /// <param name="player">The player to attack.</param>
    public override void Attack(IPlayer player)
    {
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            player.TakeDamage(collisionDamage);
            Debug.Log($"Chaser attacked player for {collisionDamage} damage.");
        }
    }

    /// <summary>
    /// Returns the score value for defeating this enemy.
    /// </summary>
    /// <returns>The score value.</returns>
    public override int GetScoreValue() => 50;

    protected override void Update()
    {
        base.Update();
        ChasePlayer();
        if (target != null && enemyState == EnemyState.Chasing && target.GetComponent<IPlayer>() != null)
        {
            Attack(target.GetComponent<IPlayer>());
        }
    }

    /// <summary>
    /// Handles collision with the player.
    /// </summary>
    /// <param name="collision">The collision data.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IPlayer player = collision.gameObject.GetComponent<IPlayer>();
            if (player != null)
            {
                player.TakeDamage(collisionDamage);
                Debug.Log($"Chaser collided with player, dealing {collisionDamage} damage.");
            }
        }
    }
}
