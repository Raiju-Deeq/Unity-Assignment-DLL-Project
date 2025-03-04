using UnityEngine;
using GamePlayDLL;
// This script defines the Shooter enemy type, which moves towards the player and shoots projectiles when in range.
public class Shooter : MonoBehaviour, IEnemy
{
    [Header("Shooter Settings")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float shootingRange = 5f;
    [SerializeField] private float fireRate = 1f; // Time between shots
    [SerializeField] private GameObject projectilePrefab; // The projectile to shoot
    [SerializeField] private Transform firePoint; // Where the projectile spawns

    private Transform player; // Reference to the player
    private float nextFireTime = 0f;

    private void Start()
    {
        // Find the player in the scene
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the Player GameObject has the 'Player' tag.");
        }
    }

    private void Update()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > shootingRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            ShootAtPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    private void ShootAtPlayer()
    {
        // Shoot at the player if cooldown has passed
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(player.position);
                }
            }
            else
            {
                Debug.LogError("Projectile Prefab or Fire Point is not assigned!");
            }
        }
    }

    public void Attack(IPlayer player)
    {
        Debug.Log("Shooter attacks!");
    }

    public int GetScoreValue() => 50;
}
