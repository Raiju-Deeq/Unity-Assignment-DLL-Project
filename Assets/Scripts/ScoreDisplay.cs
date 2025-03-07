using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the display of the player's score in the UI.
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    private Text scoreText; 
    private Player player; 

    /// <summary>
    /// Initializes the ScoreDisplay
    /// </summary>
    private void Start()
    {
        // Get the Text component attached to this GameObject
        scoreText = GetComponent<Text>();
        if (scoreText == null)
        {
            Debug.LogError("Text component not found on this GameObject");
        }

        // Find the Player in the scene
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            // Subscribe to the player's score change event
            player.OnScoreChanged += UpdateScoreDisplay;
            // Initialize the score display
            UpdateScoreDisplay(player.GetScore());
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }

    /// <summary>
    /// Updates the score display with the new score.
    /// </summary>
    /// <param name="newScore">The new score to display.</param>
    private void UpdateScoreDisplay(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
            Debug.Log($"Score updated: {newScore}");
        }
        else
        {
            Debug.LogError("ScoreText is null");
        }
    }

    /// <summary>
    /// Unsubscribes from the player's score change event when this component is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnScoreChanged -= UpdateScoreDisplay;
        }
    }
}