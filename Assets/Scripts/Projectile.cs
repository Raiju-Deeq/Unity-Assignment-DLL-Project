using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Represents a projectile in the game that moves towards a target and deals damage on impact.
/// This script should be attached to projectile game objects.
/// </summary>
public class Projectile : MonoBehaviour
{
    public float lifeTime = 3.0f; // Duration before the projectile self-destructs
    public float moveSpeed = 40.0f; // Speed at which the projectile moves
    private int damage = 2; // Amount of damage the projectile deals
    private Vector3 targetPosition; // The position the projectile is moving towards

    /// <summary>
    /// Sets the target position for the projectile.
    /// </summary>
    /// <param name="target">The Vector3 position to move towards.</param>
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    /// <summary>
    /// Sets the damage value for the projectile.
    /// </summary>
    /// <param name="newDamage">The new damage value.</param>
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    /// <summary>
    /// Initializes the projectile and sets its lifetime.
    /// </summary>
    private void Start()
    {
        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// Updates the projectile's position each frame.
    /// </summary>
    private void Update()
    {
        // Move the projectile towards the target
        if (targetPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            // Destroy the projectile if it reaches its target
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Handles collision with other game objects, specifically the player.
    /// </summary>
    /// <param name="other">The Collider of the object this projectile collided with.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IPlayer player = other.GetComponent<IPlayer>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"Projectile hit player for {damage} damage.");
            }
            Destroy(this.gameObject);
        }
    }
}
