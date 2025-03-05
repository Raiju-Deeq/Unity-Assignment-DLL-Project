using UnityEngine;
using GamePlayDLL;
public class Projectile : MonoBehaviour
{
    public float lifeTime = 3.0f;
    public float moveSpeed = 40.0f;
    private int damage = 2;
    private Vector3 targetPosition;

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void Start()
    {
        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the projectile towards the target
        if (targetPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

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