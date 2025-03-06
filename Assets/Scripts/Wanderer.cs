using UnityEngine;
using GamePlayDLL;

public class Wanderer : MonoBehaviour, IEnemy
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private int randomPoints = 5;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float startWaitTime = 1f;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float morphedMovementSpeed = 5f; // Speed when morphed
    [SerializeField] private Color normalColor = Color.blue;
    [SerializeField] private Color morphedColor = Color.red;
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 morphedScale = new Vector3(1.5f, 1.5f, 1.5f); // Size when morphed
    [SerializeField] private float morphRange = 10f; // Range to trigger morphing
    [SerializeField] private float revertRange = 15f; // Range to revert from morphed state

    private int currentPointIndex = 0;
    private float currentWaitTime;
    private bool isMorphed = false;
    private Transform playerTransform;
    private Renderer enemyRenderer;
    private float currentMovementSpeed;

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

    public void Attack(IPlayer player)
    {
        // Damage amount is higher when morphed
        int damageAmount = isMorphed ? 10 : 5;
        player.TakeDamage(damageAmount);
        Debug.Log($"Wanderer attacked player for {damageAmount} damage.");
    }

    public int GetScoreValue()
    {
        // Worth more points when morphed
        return isMorphed ? 200 : 100;
    }

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
