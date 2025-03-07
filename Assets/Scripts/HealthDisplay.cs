using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the display of the player's health in the UI.
/// </summary>
public class HealthDisplay : MonoBehaviour
{
    private Text healthText; 
    private Player player;

    /// <summary>
    /// Initializes the HealthDisplay 
    /// </summary>
    private void Start()
    {
        // Get the Text component attached to this GameObject
        healthText = GetComponent<Text>();
        if (healthText == null)
        {
            Debug.LogError("Text component not found on this GameObject");
        }
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }
        else
        {
            // Initialize the health display
            UpdateHealthDisplay();
        }
    }

    /// <summary>
    /// Updates the health display each frame.
    /// </summary>
    private void Update()
    {
        UpdateHealthDisplay();
    }

    /// <summary>
    /// Updates the health display with the player's current health.
    /// </summary>
    private void UpdateHealthDisplay()
    {
        if (player != null && healthText != null)
        {
            healthText.text = $"Health: {player.currentHealth}";
        }
    }
}