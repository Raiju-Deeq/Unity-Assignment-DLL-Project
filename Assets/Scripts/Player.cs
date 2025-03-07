using UnityEngine;
using System.Collections;
using GamePlayDLL;

/// <summary>
/// Represents the player character in the game.
/// Handles player movement, health, scoring, and power-up effects.
/// </summary>
public class Player : Character, IPlayer
{
    [SerializeField] private float maximumSpeed = 10f; // Maximum movement speed of the player
    [SerializeField] private float maxHealth = 100f; // Maximum health of the player
    [SerializeField] private Vector3 respawnPosition = Vector3.zero; // Position where the player respawns

    public event System.Action<int> OnScoreChanged; // Event triggered when the score changes

    public float currentHealth; // Current health of the player
    private bool isInvulnerable = false; // Flag to check if the player is currently invulnerable
    private int scoreMultiplier = 1; // Multiplier applied to score increases
    private int score = 0; // Current score of the player

    /// <summary>
    /// Initializes the player's health when the script awakens.
    /// </summary>
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Moves the player based on input axes.
    /// </summary>
    public void MovePlayer()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(movement * Mathf.Min(movementSpeed, maximumSpeed) * Time.deltaTime);
    }

    /// <summary>
    /// Handles the player's death and initiates respawn process.
    /// </summary>
    public void Die()
    {
        Debug.Log($"{characterName} has died.");
        StartCoroutine(RespawnCoroutine());
    }

    /// <summary>
    /// Coroutine to handle the respawn process with invulnerability period.
    /// </summary>
    private IEnumerator RespawnCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(2f);
        Respawn();
        yield return new WaitForSeconds(1f);
        isInvulnerable = false;
    }

    /// <summary>
    /// Respawns the player at the designated position with full health.
    /// </summary>
    public void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = respawnPosition;
        Debug.Log($"{characterName} has respawned with {currentHealth} health.");
    }

    // Getter and setter methods for various player properties
    public float GetMovementSpeed() => movementSpeed;
    public void SetMovementSpeed(float speed) => movementSpeed = speed;
    public bool IsInvulnerable() => isInvulnerable;
    public void SetInvulnerable(bool invulnerable) => isInvulnerable = invulnerable;
    public int GetScoreMultiplier() => scoreMultiplier;
    public void SetScoreMultiplier(int multiplier) => scoreMultiplier = multiplier;

    /// <summary>
    /// Adds points to the player's score, applying the current multiplier.
    /// </summary>
    /// <param name="points">The base number of points to add.</param>
    public void AddScore(int points)
    {
        score += points * scoreMultiplier;
        Debug.Log($"Score increased by {points}. New score: {score}");
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore() => score;

    /// <summary>
    /// Applies damage to the player if not invulnerable.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        currentHealth -= damage;
        Debug.Log($"{characterName} took {damage} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
            Die();
    }

    /// <summary>
    /// Handles collision with power-ups and applies their effects.
    /// </summary>
    /// <param name="other">The collider of the object collided with.</param>
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

    /// <summary>
    /// Applies a power-up effect to the player.
    /// </summary>
    /// <param name="effect">The power-up effect to apply.</param>
    private void ApplyPowerUp(PowerUpEffect effect)
    {
        effect.ApplyEffect(this);
        if (!(effect is CoinEffect))
            StartCoroutine(RemovePowerUpAfterDelay(effect));
    }

    /// <summary>
    /// Coroutine to remove a power-up effect after a delay.
    /// </summary>
    /// <param name="effect">The power-up effect to remove.</param>
    private IEnumerator RemovePowerUpAfterDelay(PowerUpEffect effect)
    {
        yield return new WaitForSeconds(5f);
        effect.RemoveEffect(this);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        ClampPosition();
        MovePlayer();
    }
}
