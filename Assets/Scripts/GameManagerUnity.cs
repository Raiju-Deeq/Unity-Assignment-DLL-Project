using UnityEngine;
using System.Collections;
using GamePlayDLL;
using UnityEngine.UI;

/// <summary>
/// Manages the overall game state, including spawning players, enemies, and collectables.
/// Handles game flow (start, pause, resume, end) and score display.
/// </summary>
public class GameManagerUnity : MonoBehaviour
{
    private GameManager gameManager;

    // Hazard settings
    public GameObject hazardPrefab;
    public int numberOfHazards = 5;
    public Vector3 levelBounds = new Vector3(10, 0, 10);

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject wandererPrefab;
    [SerializeField] private GameObject chaserPrefab;
    [SerializeField] private GameObject shooterPrefab;
    [SerializeField] private int numberOfEnemies = 5;
    [SerializeField] private float enemySpawnDelay = 2f;

    [Header("Spawn Area")]
    [SerializeField] private Vector3 spawnAreaMin; // Minimum bounds for spawning
    [SerializeField] private Vector3 spawnAreaMax; // Maximum bounds for spawning

    [Header("Collectable Settings")]
    [SerializeField] private GameObject[] collectablePrefabs;
    [SerializeField] private float collectableSpawnInterval = 5f;

    [Header("UI Elements")]
    [SerializeField] private Text scoreText;

    private Player player;

    /// <summary>
    /// Initializes the game, spawns entities, and sets up UI.
    /// </summary>
    private void Start()
    {
        gameManager = new GameManager();
        gameManager.StartGame();
        SpawnPlayer();
        StartCoroutine(SpawnEnemiesWithDelay());
        InvokeRepeating(nameof(SpawnCollectable), 0f, collectableSpawnInterval);
        SpawnHazards();

        player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.OnScoreChanged += UpdateScoreUI;
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }

        UpdateScoreUI(0); // Initialize score display
    }

    private void Update()
    {
        // Update logic can be added here if needed
    }

    /// <summary>
    /// Spawns hazards at random positions within the level bounds.
    /// </summary>
    private void SpawnHazards()
    {
        for (int i = 0; i < numberOfHazards; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-levelBounds.x, levelBounds.x),
                0,
                Random.Range(-levelBounds.z, levelBounds.z)
            );
            Instantiate(hazardPrefab, randomPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns the player at a random position within the spawn area.
    /// </summary>
    private void SpawnPlayer()
    {
        Vector3 playerSpawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Coroutine to spawn enemies with a delay between each spawn.
    /// </summary>
    private IEnumerator SpawnEnemiesWithDelay()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    /// <summary>
    /// Spawns a random enemy type at a random position within the spawn area.
    /// </summary>
    private void SpawnEnemy()
    {
        Vector3 enemySpawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );
        float randomValue = Random.value;
        if (randomValue < 0.33f) // 33% chance to spawn a Wanderer
        {
            Instantiate(wandererPrefab, enemySpawnPosition, Quaternion.identity);
        }
        else if (randomValue < 0.66f) // 33% chance to spawn a Chaser
        {
            Instantiate(chaserPrefab, enemySpawnPosition, Quaternion.identity);
        }
        else // Remaining chance to spawn a Shooter
        {
            Instantiate(shooterPrefab, enemySpawnPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns a random collectable at a random position within the spawn area.
    /// </summary>
    private void SpawnCollectable()
    {
        if (collectablePrefabs.Length == 0) return;
        Vector3 collectableSpawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );
        GameObject collectablePrefab = collectablePrefabs[Random.Range(0, collectablePrefabs.Length)];
        Instantiate(collectablePrefab, collectableSpawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void PauseGame()
    {
        gameManager.PauseGame();
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        gameManager.StartGame();
        Time.timeScale = 1;
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    public void EndGame()
    {
        gameManager.EndGame();
        Debug.Log("Game Over");
    }

    /// <summary>
    /// Updates the score display in the UI.
    /// </summary>
    /// <param name="newScore">The new score to display.</param>
    private void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}";
        }
        else
        {
            Debug.LogError("Score Text reference is missing!");
        }
    }
}
