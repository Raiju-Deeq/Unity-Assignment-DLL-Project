using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private Spawner spawner; // Reference to the Spawner component

        [Header("Game State")]
        public bool isGameRunning = false; // Tracks whether the game is running

        private void Start()
        {
            if (spawner == null)
            {
                Debug.LogError("Spawner is not assigned in Level Manager!");
                return;
            }

            StartGame();
        }

        /// <summary>
        /// Starts the game and initializes all gameplay elements.
        /// </summary>
        public void StartGame()
        {
            isGameRunning = true;
            Debug.Log("Game Started!");

            spawner.StartSpawning(); // Start spawning collectables and enemies
            // Additional initialization logic can go here (e.g., resetting score)
        }

        /// <summary>
        /// Ends the game and stops all gameplay elements.
        /// </summary>
        public void EndGame()
        {
            isGameRunning = false;
            Debug.Log("Game Over!");

            spawner.StopSpawning(); // Stop spawning collectables and enemies
            // Additional game-over logic can go here (e.g., displaying Game Over UI)
        }
    }
}