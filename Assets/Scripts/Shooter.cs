using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Represents a shooter enemy that moves towards the player and fires projectiles.
/// </summary>
public class Shooter : MonoBehaviour, IEnemy
{
    [Header("Shooter Settings")]
    [SerializeField] private float movementSpeed = 2f; 
    [SerializeField] private float shootingRange = 5f; 
    [SerializeField] private float fireRate = 1f; 
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform firePoint; 
    [SerializeField] private int projectileDamage = 2; 

    private Transform player; 
    private float nextFireTime = 0f; 

    /// <summary>
    /// Initializes the shooter by finding the player in the scene.
    /// </summary>
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

    /// <summary>
    /// Updates the shooter's behavior each frame.
    /// </summary>
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

    /// <summary>
    /// Moves the shooter towards the player.
    /// </summary>
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Shooting behavior 
    /// </summary>
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
                    projectileScript.SetDamage(projectileDamage);
                    Debug.Log($"Shooter fired a projectile with {projectileDamage} damage.");
                }
            }
            else
            {
                Debug.LogError("Projectile Prefab or Fire Point is not assigned!");
            }
        }
    }

    /// <summary>
    /// Implements the Attack method from the IEnemy interface.
    /// </summary>
    /// <param name="player">The player to attack.</param>
    public void Attack(IPlayer player)
    {
        Debug.Log("Shooter attacks!");
    }

    /// <summary>
    /// Returns the score value for defeating this enemy.
    /// </summary>
    /// <returns>The score value.</returns>
    /// Todo when I add player shooting ability the player will get 50 points for killing this enemy
    public int GetScoreValue() => 50;
}
