using UnityEngine;
using UML;

/// <summary>
/// Handles player input and delegates functionality to the Player class.
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player player; // Reference to the Player class

    private void Awake()
    {
        // Get reference to the Player component
        player = GetComponent<Player>();
    }

    private void Update()
    {
        HandleMovement();
        HandleHealthDebug(); // Optional: For testing health management
    }

    /// <summary>
    /// Handles player movement based on input.
    /// Delegates movement logic to the Player class.
    /// </summary>
    private void HandleMovement()
    {
        player.MovePlayer();
    }

    /// <summary>
    /// Debugging method to simulate taking damage using a key press.
    /// </summary>
    private void HandleHealthDebug()
    {
        if (Input.GetKeyDown(KeyCode.H)) // Press 'H' to take damage
        {
            player.TakeDamage(10f);
        }
    }
}