using UnityEngine;
using System.Collections;
using GamePlayDLL;

public class Player : Character, IPlayer
{
    [SerializeField] private float maximumSpeed = 10f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Vector3 respawnPosition = Vector3.zero;

    private float currentHealth;
    private ScoreManager scoreManager;
    private PowerUpManager powerUpManager;
    private bool isInvulnerable = false;

    private void Awake()
    {
        scoreManager = new ScoreManager();
        powerUpManager = new PowerUpManager();
        currentHealth = maxHealth;
    }

    public void MovePlayer()
    {
        // Calculate movement based on input
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Apply movement, clamped to maximum speed
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
        // Wait for 2 seconds before respawning
        yield return new WaitForSeconds(2f);
        Respawn();
        // 1 second of invulnerability after respawn
        yield return new WaitForSeconds(1f);
        isInvulnerable = false;
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnPosition;
        Debug.Log($"{characterName} has respawned with {currentHealth} health.");
    }

    public void ApplyPowerUp(PowerUpType type, float duration)
    {
        powerUpManager.ApplyPowerUp(this, type, duration);
        Debug.Log($"Applied power-up: {type} for {duration} seconds.");
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        Debug.Log($"{characterName} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddScore(int points)
    {
        scoreManager.AddScore(points);
        Debug.Log($"Score increased by {points}. New score: {scoreManager.GetCurrentScore()}");
    }

    public int GetScore()
    {
        return scoreManager.GetCurrentScore();
    }

    private void Update()
    {
        MovePlayer();
        powerUpManager.UpdatePowerUps();
    }
}
