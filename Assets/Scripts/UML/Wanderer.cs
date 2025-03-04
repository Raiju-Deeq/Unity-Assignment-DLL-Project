using UnityEngine;
namespace UML
{
    public class Wanderer : Enemy
{
    [SerializeField] private Transform[] patrolPoints; // Array of patrol points
    [SerializeField] private int randomPoints = 0; // Number of random points to generate
    [SerializeField] private float waitTime = 2f; // Time to wait at each patrol point
    private float waitTimer;

    private int currentPointIndex = 0;

    private void Start()
    {
        waitTimer = waitTime;
        enemyState = EnemyState.Patrolling;

        // Generate random patrol points if specified
        if (randomPoints > 0)
        {
            patrolPoints = new Transform[randomPoints];
            for (int i = 0; i < randomPoints; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-10f, 10f),
                    0,
                    Random.Range(-10f, 10f)
                );
                GameObject point = new GameObject($"PatrolPoint_{i}");
                point.transform.position = randomPosition;
                patrolPoints[i] = point.transform;
            }
        }

        // Start at the first patrol point if available
        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
        }
    }

    private void Update()
    {
        if (enemyState == EnemyState.Patrolling)
        {
            Patrol();
        }
    }

    /// <summary>
    /// Patrols between predefined or randomly generated points.
    /// </summary>
    public void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, movementSpeed * Time.deltaTime);

        // Check if the Wanderer has reached the current patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (waitTimer <= 0)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length; // Move to the next point
                waitTimer = waitTime; // Reset wait timer
            }
            else
            {
                waitTimer -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Morphs or changes behavior (placeholder for additional functionality).
    /// </summary>
    public void Morph()
    {
        Debug.Log($"{GetName()} is morphing!");
        // Implement morphing logic here, such as changing appearance or behavior.
    }

    protected override void Move()
    {
        Patrol(); // The Wanderer primarily moves by patrolling.
    }
}
}