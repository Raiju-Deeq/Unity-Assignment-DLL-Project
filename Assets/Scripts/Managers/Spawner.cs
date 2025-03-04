using GamePlayDLL;
using UML;
using UnityEngine;

namespace Managers
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private Vector3 minSpawnPosition; // Minimum position for spawning
        [SerializeField] private Vector3 maxSpawnPosition; // Maximum position for spawning
        [SerializeField] private float spawnInterval = 5f; // Time interval between spawns

        [Header("Prefabs")]
        [SerializeField] private GameObject[] collectablePrefabs; // Array of collectable prefabs
        [SerializeField] private GameObject[] enemyPrefabs;       // Array of enemy prefabs

        private SpawningManager _spawningManager; // Reference to the DLL's SpawningManager

        private void Awake()
        {
            // Initialize the SpawningManager from the DLL
            _spawningManager = new SpawningManager();
        }

        /// <summary>
        /// Starts spawning objects at regular intervals.
        /// </summary>
        public void StartSpawning()
        {
            InvokeRepeating(nameof(SpawnCollectable), 0f, spawnInterval);
            InvokeRepeating(nameof(SpawnEnemy), spawnInterval / 2, spawnInterval); // Enemies spawn at a different interval
        }

        /// <summary>
        /// Stops all spawning.
        /// </summary>
        public void StopSpawning()
        {
            CancelInvoke(nameof(SpawnCollectable));
            CancelInvoke(nameof(SpawnEnemy));
        }

        /// <summary>
        /// Spawns a random collectable within the defined bounds.
        /// </summary>
        private void SpawnCollectable()
        {
            if (collectablePrefabs.Length == 0) return;

            // Select a random collectable prefab
            GameObject prefabToSpawn = collectablePrefabs[Random.Range(0, collectablePrefabs.Length)];

            // Use the DLL's SpawningManager to spawn the object
            GameObject spawnedObject = _spawningManager.Spawn(prefabToSpawn, minSpawnPosition, maxSpawnPosition);

            if (spawnedObject != null)
            {
                Debug.Log($"Spawned Collectable: {spawnedObject.name}");
                spawnedObject.AddComponent<CollectableItem>(); // Add interaction logic
            }
        }

        /// <summary>
        /// Spawns a random enemy within the defined bounds.
        /// </summary>
        private void SpawnEnemy()
        {
            if (enemyPrefabs.Length == 0) return;

            // Select a random enemy prefab
            GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Use the DLL's SpawningManager to spawn the object
            GameObject spawnedObject = _spawningManager.Spawn(prefabToSpawn, minSpawnPosition, maxSpawnPosition);

            if (spawnedObject != null)
            {
                Debug.Log($"Spawned Enemy: {spawnedObject.name}");
                Enemy enemyScript = spawnedObject.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.target = FindObjectOfType<Player>().transform; // Set player as target for enemies
                }
            }
        }
    }
}