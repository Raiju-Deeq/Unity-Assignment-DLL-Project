using UnityEngine;
using GameplayPlugin; // Important: Use the DLL's namespace
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager? Instance { get; private set; }
    public GameObject[] enemyPrefabs = null!;
    public GameObject[] collectablePrefabs = null!;
    public float spawnAreaSize = 10f;

    public TextMeshProUGUI scoreText = null!;
    public TextMeshProUGUI livesText = null!;

    private int lives = 3;

    private GameplayPlugin.GameManager? pluginGameManager;
    private GameplayPlugin.SpawningManager? spawningManager;

    public event Action<int>? OnLivesChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Initialize the ScoringSystem and SpawningManager from the DLL
        pluginGameManager = new GameplayPlugin.GameManager();
        spawningManager = gameObject.AddComponent<GameplayPlugin.SpawningManager>();
        spawningManager.spawnAreaSize = spawnAreaSize;

        // Add enemy prefabs to the SpawningManager
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            spawningManager.AddEnemyType(enemyPrefab);
        }

        // Add collectable prefabs to the SpawningManager
        foreach (GameObject collectablePrefab in collectablePrefabs)
        {
            spawningManager.AddCollectableType(collectablePrefab);
        }

        //UI Initialization
        UpdateScoreText(0);
        UpdateLivesText();

        // Subscribe to the OnScoreUpdate event
        if (GameplayPlugin.GameManager.Instance != null)
        {
            GameplayPlugin.GameManager.Instance.OnScoreUpdate += UpdateScoreText;
        }

        OnLivesChanged += UpdateLivesText;
    }
    public void AddScore(int points)
    {
        pluginGameManager?.AddScore(points); // Call the AddScore method in the DLL

    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("Score Text not assigned. Please assign a Text object in the Inspector.");
        }
    }

    public void RemoveLife()
    {
        lives--;
        if (OnLivesChanged != null)
        {
            OnLivesChanged(lives);
        }

        if (lives <= 0)
        {
            pluginGameManager?.SetGameState(GameplayPlugin.GameState.GameOver);
            Debug.Log("Game Over!");
        }
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives.ToString();
        }
        else
        {
            Debug.LogError("Lives Text not assigned. Please assign a Text object in the Inspector.");
        }
    }

    private void UpdateLivesText(int currentLives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives.ToString();
        }
        else
        {
            Debug.LogError("Lives Text not assigned. Please assign a Text object in the Inspector.");
        }
    }
}
