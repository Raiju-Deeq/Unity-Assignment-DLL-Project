using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
/// Manages the game's UI elements, including health, score, and game state.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text _healthText;       // UI text to display player's health
    [SerializeField] private Text _scoreText;        // UI text to display player's score
    [SerializeField] private GameObject _gameOverPanel; // UI panel to display "Game Over" screen

    private Player _player;           // Reference to the Player object
    private GameManager _gameManager; // Reference to the GameManager (formerly LevelManager)
    private int _playerScore;         // Tracks the player's score

    private void Start()
    {
        // Find references to Player and GameManager
        _player = FindObjectOfType<Player>();
        _gameManager = FindObjectOfType<GameManager>();

        if (_player == null)
        {
            Debug.LogError("Player not found in the scene!");
            return;
        }

        if (_gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }

        // Initialize UI elements
        UpdateHealthDisplay();
        UpdateScoreDisplay();
        _gameOverPanel.SetActive(false); // Hide Game Over panel at the start
    }

    private void Update()
    {
        // Continuously update health display during gameplay
        UpdateHealthDisplay();

        // Check if the game is over (based on GameManager state)
        if (!_gameManager.isGameRunning && !_gameOverPanel.activeSelf)
        {
            ShowGameOverScreen();
        }
    }

    /// <summary>
    /// Updates the health display based on the player's current health.
    /// </summary>
    private void UpdateHealthDisplay()
    {
        if (_player != null)
        {
            _healthText.text = $"Health: {_player.GetHealth()}";
        }
    }

    /// <summary>
    /// Updates the score display based on the current score.
    /// </summary>
    private void UpdateScoreDisplay()
    {
        _scoreText.text = $"Score: {_playerScore}";
    }

    /// <summary>
    /// Increments the player's score and updates the UI.
    /// </summary>
    /// <param name="points">The number of points to add.</param>
    public void AddScore(int points)
    {
        _playerScore += points;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Displays the Game Over screen when the game ends.
    /// </summary>
    private void ShowGameOverScreen()
    {
        _gameOverPanel.SetActive(true);
        Debug.Log("Game Over! Displaying Game Over screen.");
    }
}
}