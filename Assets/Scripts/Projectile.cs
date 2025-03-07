using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Projectile might use on player too currently only used by Shooter class
/// </summary>
public class Projectile : MonoBehaviour
{
    public float lifeTime = 3.0f; 
    public float moveSpeed = 40.0f; 
    private int damage = 2; 
    private Vector3 targetPosition; 
    /// <summary>
    /// TODO work on object pool again lost when merging branches
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
    /// Update projectile
    /// </summary>
    private void Update()
    {
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
    /// Handles collision with other game objects
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
