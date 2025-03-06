using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Represents a wandering enemy that patrols between points and can morph when near the player.
/// This script should be attached to wanderer enemy game objects.
/// </summary>
public class Wanderer : MonoBehaviour, IEnemy
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints; // Array of patrol points
    [SerializeField] private int randomPoints = 5; // Number of random points to generate if no patrol points are set
    [SerializeField] private float waitTime = 1f; // Time to wait at each patrol point
    [SerializeField] private float startWaitTime = 1f; // Initial wait time

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f; // Normal movement speed
    [SerializeField] private float morphedMovementSpeed = 5f; // Speed when morphed

    [Header("Appearance Settings")]
    [SerializeField] private Color normalColor = Color.blue; // Color in normal state
    [SerializeField] private Color morphedColor = Color.red; // Color when morphed
    [SerializeField] private Vector3 normalScale = Vector3.one; // Size in normal state
    [SerializeField] private Vector3 morphedScale = new Vector3(1.5f, 1.5f, 1.5f); // Size when morphed

    [Header("Morph Settings")]
    [SerializeField] private float morphRange = 10f; // Range to trigger morphing
    [SerializeField] private float revertRange = 15f; // Range to revert from morphed state

    private int currentPointIndex = 0; // Index of the current patrol point
    private float currentWaitTime; // Current wait time at a patrol point
    private bool isMorphed = false; // Flag to check if the wanderer is morphed
    private Transform playerTransform; // Reference to the player's transform
    private Renderer enemyRenderer; // Reference to the enemy's renderer component
    private float currentMovementSpeed; // Current movement speed

    /// <summary>
    /// Initializes the wanderer by finding the player, setting up components, and generating patrol points if needed.
    /// </summary>
    private void Start()
    {
        // Find the player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // Get the renderer component for color changing
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = normalColor;
        }

        // Set initial values
        currentWaitTime = startWaitTime;
        currentMovementSpeed = movementSpeed;
        transform.localScale = normalScale;

        // Generate random patrol points if none are assigned
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            GenerateRandomPatrolPoints();
        }
    }

    /// <summary>
    /// Generates random patrol points within a specified range.
    /// </summary>
    private void GenerateRandomPatrolPoints()
    {
        patrolPoints = new Transform[randomPoints];
        for (int i = 0; i < randomPoints; i++)
        {
            GameObject point = new GameObject($"PatrolPoint_{i}");
            point.transform.position = new Vector3(
                Random.Range(-10f, 10f),
                transform.position.y,
                Random.Range(-10f, 10f)
            );
            patrolPoints[i] = point.transform;
        }
    }

    /// <summary>
    /// Updates the wanderer's behavior each frame, checking for morphing conditions and patrolling.
    /// </summary>
    private void Update()
    {
        // Check distance to player for morphing
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            // Morph if player is within range and not already morphed
            if (distanceToPlayer <= morphRange && !isMorphed)
            {
                Morph();
            }
            // Revert if player is out of range and currently morphed
            else if (distanceToPlayer > revertRange && isMorphed)
            {
                RevertFromMorph();
            }
        }

        // Continue patrolling
        Patrol();
    }

    /// <summary>
    /// Handles the patrolling behavior of the wanderer.
    /// </summary>
    public void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Move towards the current patrol point
        transform.position = Vector3.MoveTowards(
            transform.position,
            patrolPoints[currentPointIndex].position,
            currentMovementSpeed * Time.deltaTime
        );

        // Check if reached the current patrol point
        if (Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.2f)
        {
            // Wait at the patrol point
            if (currentWaitTime <= 0)
            {
                // Move to the next patrol point
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Morphs the wanderer, changing its appearance and speed.
    /// </summary>
    public void Morph()
    {
        isMorphed = true;
        // Change appearance
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = morphedColor;
        }
        // Increase size
        transform.localScale = morphedScale;
        // Increase speed
        currentMovementSpeed = morphedMovementSpeed;
        Debug.Log("Wanderer has morphed!");
    }

    /// <summary>
    /// Reverts the wanderer from its morphed state.
    /// </summary>
    public void RevertFromMorph()
    {
        isMorphed = false;
        // Revert appearance
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = normalColor;
        }
        // Revert size
        transform.localScale = normalScale;
        // Revert speed
        currentMovementSpeed = movementSpeed;
        Debug.Log("Wanderer has reverted from morphed state.");
    }

    /// <summary>
    /// Implements the Attack method from the IEnemy interface.
    /// </summary>
    /// <param name="player">The player to attack.</param>
    public void Attack(IPlayer player)
    {
        // Damage amount is higher when morphed
        int damageAmount = isMorphed ? 10 : 5;
        player.TakeDamage(damageAmount);
        Debug.Log($"Wanderer attacked player for {damageAmount} damage.");
    }

    /// <summary>
    /// Returns the score value for defeating this enemy.
    /// </summary>
    /// <returns>The score value, which is higher when morphed.</returns>
    public int GetScoreValue()
    {
        // Worth more points when morphed
        return isMorphed ? 200 : 100;
    }

    /// <summary>
    /// Handles collision with the player.
    /// </summary>
    /// <param name="other">The Collider of the object this wanderer collided with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IPlayer player = other.GetComponent<IPlayer>();
            if (player != null)
            {
                Attack(player);
            }
        }
    }
}
