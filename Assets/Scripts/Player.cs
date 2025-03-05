using System;
using UnityEngine;
using GamePlayDLL;
// This script defines the Player class, handling player movement, health, power-ups, and other player-specific behaviors.
public class Player : Character, IPlayer
{
    private ScoreManager scoreManager;
    private PowerUpManager powerUpManager;

    [SerializeField] private float maximumSpeed;
    [SerializeField] private float healthCount;

    private void Awake()
    {
        scoreManager = new ScoreManager();
        powerUpManager = new PowerUpManager();
    }
    
    public void MovePlayer()
    {
        // Implement player movement logic
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(movement * movementSpeed * Time.deltaTime);
    }

    public void Die()
    {
        // Implement death logic
        Debug.Log($"{characterName} has died.");
        // Trigger game over state
    }

    public void Respawn()
    {
        // Implement respawn logic
        healthCount = 100f;
        transform.position = Vector3.zero;
        Debug.Log($"{characterName} has respawned.");
    }

    public void ApplyPowerUp(PowerUpType type, float duration)
    {
        powerUpManager.ApplyPowerUp(this, type, duration);
    }

    public void TakeDamage(int damage)
    {
        healthCount -= damage;
        if (healthCount <= 0)
        {
            Die();
        }
    }

    public void AddScore(int points)
    {
        scoreManager.AddScore(points);
    }

    public int GetScore()
    {
        return scoreManager.GetCurrentScore();
    }

    private void Update()
    {
        ClampPosition();
        MovePlayer();
        powerUpManager.UpdatePowerUps();
    }
}