using UnityEngine;
using GamePlayDLL;
// This script defines the Chaser enemy type, which actively chases and attacks the player when in range.
public class Chaser : Enemy
{
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float attackRange = 2f;

    private void Start()
    {
        // Find and set the player as the target
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ChasePlayer()
    {
        // Set enemy state based on distance to player
        if (Vector3.Distance(transform.position, target.position) <= chaseRange)
        {
            enemyState = EnemyState.Chasing;
        }
        else
        {
            enemyState = EnemyState.Idle;
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
            player.TakeDamage(10);
        }
    }

    public override int GetScoreValue() => 50;

    protected override void Update()
    {
        base.Update();
        ChasePlayer();
        if (target != null && target.GetComponent<IPlayer>() != null)
        {
            Attack(target.GetComponent<IPlayer>());
        }
    }
}