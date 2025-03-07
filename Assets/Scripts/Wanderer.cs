using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Represents a wandering enemy that patrols between points and can morph when near the player.
/// </summary>
public class Wanderer : MonoBehaviour, IEnemy
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private int randomPoints = 5; 
    [SerializeField] private float waitTime = 1f; 
    [SerializeField] private float startWaitTime = 1f; 

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f; 
    [SerializeField] private float morphedMovementSpeed = 5f; 

    [Header("Appearance Settings")]
    [SerializeField] private Color normalColor = Color.blue; 
    [SerializeField] private Color morphedColor = Color.red; 
    [SerializeField] private Vector3 normalScale = Vector3.one; 
    [SerializeField] private Vector3 morphedScale = new Vector3(3f, 3f, 3f); 

    [Header("Morph Settings")]
    [SerializeField] private float morphRange = 10f;
    [SerializeField] private float revertRange = 15f; 

    private int currentPointIndex = 0; 
    private float currentWaitTime; 
    private bool isMorphed = false; 
    private Transform playerTransform; 
    private Renderer enemyRenderer; 
    private float currentMovementSpeed; 

    /// <summary>
    /// Initializes the wanderer
    /// </summary>
    private void Start()
    {
        
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
    /// Updates the wanderer's behavior each frame, patrol and check for morph conditions
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
    /// patrolling behaviour
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

        
        if (Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.2f)
        {
            
            if (currentWaitTime <= 0)
            {
                
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
    /// Morphs the wanderer
    /// </summary>
    public void Morph()
    {
        isMorphed = true;
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = morphedColor;
        }
        transform.localScale = morphedScale;
        currentMovementSpeed = morphedMovementSpeed;
        Debug.Log("Wanderer has morphed!");
    }

    /// <summary>
    /// Reverts the wanderer from its morphed state.
    /// </summary>
    public void RevertFromMorph()
    {
        isMorphed = false;
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = normalColor;
        }
        transform.localScale = normalScale;
        currentMovementSpeed = movementSpeed;
        Debug.Log("Wanderer has reverted from morphed state.");
    }

    /// <summary>
    /// Implements the Attack method from the IEnemy interface. Not used fully atm
    /// TODO add shooting behaviour
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
    /// Returns the score value for defeating this enemy.not implemented properly
    /// TODO add killing enemy functionality
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
