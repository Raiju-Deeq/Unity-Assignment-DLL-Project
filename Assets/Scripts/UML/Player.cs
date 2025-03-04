using GamePlayDLL;
using UML;
using UnityEngine;

/// <summary>
/// Represents the player character in the game.
/// Inherits from Character and implements movement, health management, and respawn functionality.
/// </summary>
public class Player : Character
{
    [Header("Player Attributes")]
    [SerializeField] private float _maximumSpeed = 10f; // Maximum speed of the player
    [SerializeField] private float _healthCount = 100f; // Player's health

    private Vector3 _movementInput; // Stores movement input for the player

    /// <summary>
    /// Handles player movement every frame.
    /// </summary>
    private void Update()
    {
        MovePlayer();
    }

    /// <summary>
    /// Moves the player based on input.
    /// </summary>
    public void MovePlayer()
    {
        // Get input from keyboard or controller
        _movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Normalize input to prevent diagonal speed increase
        if (_movementInput.magnitude > 1)
        {
            _movementInput = _movementInput.normalized;
        }

        // Calculate movement vector
        Vector3 move = _movementInput * (movementSpeed * Time.deltaTime);

        // Apply movement to the player's position
        transform.Translate(move, Space.World);

        // Clamp speed to maximumSpeed
        if (movementSpeed > _maximumSpeed)
        {
            movementSpeed = _maximumSpeed;
        }
    }

    /// <summary>
    /// Reduces the player's health by a specified amount.
    /// If health reaches zero, triggers the Die() method.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(float damage)
    {
        _healthCount -= damage;
        Debug.Log($"{GetName()} took {damage} damage. Remaining health: {_healthCount}");

        if (_healthCount <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles player death logic.
    /// </summary>
    private void Die()
    {
        Debug.Log($"{GetName()} has died!");

        // Disable the player object (can be replaced with game-over logic)
        gameObject.SetActive(false);

        // Trigger respawn or game-over logic here
        Respawn(new Vector3(0, 0, 0)); // Example respawn position (can be replaced with a specific location)
    }

    /// <summary>
    /// Respawns the player at a specific location.
    /// Resets health and reactivates the player object.
    /// </summary>
    /// <param name="respawnPosition">The position where the player will respawn.</param>
    private void Respawn(Vector3 respawnPosition)
    {
        transform.position = respawnPosition; // Move player to respawn position
        _healthCount = 100f;                  // Reset health to full
        gameObject.SetActive(true);           // Reactivate player object

        Debug.Log($"{GetName()} has respawned at {respawnPosition}!");
    }

    /// <summary>
    /// Gets the current health of the player.
    /// </summary>
    /// <returns>The player's current health.</returns>
    public float GetHealth()
    {
        return _healthCount;
    }

    protected override void Move()
    {
        MovePlayer(); // Use MovePlayer() as the implementation for abstract Move() method from Character
    }
}