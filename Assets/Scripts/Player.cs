using UnityEngine;
using System.Collections;
using GamePlayDLL;

public class Player : Character, IPlayer
{
    [SerializeField] private float maximumSpeed = 10f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Vector3 respawnPosition = Vector3.zero;

    private float currentHealth;
     // Default movement speed
    private bool isInvulnerable = false;
    private int scoreMultiplier = 1;
    private int score = 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(movement * Mathf.Min(movementSpeed, maximumSpeed) * Time.deltaTime);
    }

    public void Die()
    {
        Debug.Log($"{characterName} has died.");
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(2f);
        Respawn();
        yield return new WaitForSeconds(1f);
        isInvulnerable = false;
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnPosition;
        Debug.Log($"{characterName} has respawned with {currentHealth} health.");
    }

    public float GetMovementSpeed() => movementSpeed;

    public void SetMovementSpeed(float speed) => movementSpeed = speed;

    public bool IsInvulnerable() => isInvulnerable;

    public void SetInvulnerable(bool invulnerable) => isInvulnerable = invulnerable;

    public int GetScoreMultiplier() => scoreMultiplier;

    public void SetScoreMultiplier(int multiplier) => scoreMultiplier = multiplier;

    public void AddScore(int points)
    {
        score += points * scoreMultiplier;
        Debug.Log($"Score increased by {points}. New score: {score}");
    }

    public int GetScore() => score;

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log($"{characterName} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
            ApplyPowerUp(new SpeedBoostEffect());
        else if (other.CompareTag("Shield"))
            ApplyPowerUp(new ShieldEffect());
        else if (other.CompareTag("DoublePoints"))
            ApplyPowerUp(new DoublePointsEffect());
        else if (other.CompareTag("Coin"))
            ApplyPowerUp(new CoinEffect());

        Destroy(other.gameObject);
    }

    private void ApplyPowerUp(PowerUpEffect effect)
    {
        effect.ApplyEffect(this);
        if (!(effect is CoinEffect))
            StartCoroutine(RemovePowerUpAfterDelay(effect));
    }

    private IEnumerator RemovePowerUpAfterDelay(PowerUpEffect effect)
    {
        yield return new WaitForSeconds(30f);
        effect.RemoveEffect(this);
    }

    private void Update()
    {
        ClampPosition();
        MovePlayer();
    }
}
