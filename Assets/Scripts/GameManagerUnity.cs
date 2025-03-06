using UnityEngine;
using System.Collections;
using GamePlayDLL;
using UnityEngine.UI;
// This script manages the overall game state, including spawning players, enemies, and collectables, as well as handling game flow (start, pause, resume, end) and score display.
public class GameManagerUnity : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject hazardPrefab;
    public int numberOfHazards = 5;
    public Vector3 levelBounds = new Vector3(10, 0, 10);

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject wandererPrefab;
    [SerializeField] private GameObject chaserPrefab;
    [SerializeField] private GameObject shooterPrefab; // Added Shooter prefab
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

    private void Start()
    {
        gameManager = new GameManager();
        gameManager.StartGame();

        SpawnPlayer();
        StartCoroutine(SpawnEnemiesWithDelay());
        InvokeRepeating(nameof(SpawnCollectable), 0f, collectableSpawnInterval);
        SpawnHazards();
        UpdateScoreUI();
    }

    private void Update()
    {
        UpdateScoreUI();
    }

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

    private void SpawnPlayer()
    {
        Vector3 playerSpawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y),
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    }

    private IEnumerator SpawnEnemiesWithDelay()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

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

    public void PauseGame()
    {
        gameManager.PauseGame();
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gameManager.StartGame();
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        gameManager.EndGame();
        Debug.Log("Game Over");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {gameManager.GetCurrentScore()}";
        }
    }

}
