using UnityEngine;
using GamePlayDLL;

public class Chaser : Enemy
{
    [SerializeField] private float chaseRange = 15f; // Updated to 15 units
    [SerializeField] private float idleRange = 20f; // New range for idle state
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int collisionDamage = 10;
    [SerializeField] private Color normalColor = Color.red; // Default color when chasing
    [SerializeField] private Color idleColor = Color.grey; // Grey color when idle

    private Renderer enemyRenderer;

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

    public void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // Set enemy state based on distance to player
        if (distanceToPlayer <= chaseRange)
        {
            enemyState = EnemyState.Chasing;

            // Change color to normal when chasing
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

            // Change color to grey when idle
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = idleColor;
            }

            // Don't move when idle (beyond 20 units)
        }
    }

    public void Retreat()
    {
        // Move away from the player
        transform.position = Vector3.MoveTowards(transform.position, target.position, -movementSpeed * Time.deltaTime);
    }

    public override void Attack(IPlayer player)
    {
        // Attack the player if within range
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            player.TakeDamage(collisionDamage);
            Debug.Log($"Chaser attacked player for {collisionDamage} damage.");
        }
    }

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
