using UnityEngine;

public class Wanderer : Enemy
{
    public Transform[]? patrolPoints; // Array of patrol points
    public int randomPoints = 3;

    public float waitTime = 2f;
    public float startWaitTime = 1f;

    private int currentPointIndex = 0;

    void Start()
    {
        // Initialize patrol points array if it's null
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            patrolPoints = new Transform[randomPoints];
            // Generate random patrol points within a certain radius
            for (int i = 0; i < randomPoints; i++)
            {
                // Calculate a random direction
                Vector2 randomDirection = Random.insideUnitCircle.normalized;

                // Calculate the random position within a radius of 10 units
                Vector3 randomPosition = transform.position + new Vector3(randomDirection.x, 0, randomDirection.y) * 10f;

                // Create a new GameObject for the patrol point
                GameObject patrolPointObj = new GameObject("PatrolPoint_" + i);
                patrolPointObj.transform.position = randomPosition;
                patrolPointObj.transform.parent = transform; // Make it a child of the Wanderer for organization

                // Assign the transform to the patrolPoints array
                patrolPoints[i] = patrolPointObj.transform;
            }
        }
    }

    public void Patrol()
    {
        enemyState = EnemyState.Patrolling;
        // Implement Patrol logic here (e.g., move between patrol points)
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned to Wanderer.");
            return;
        }

        // Check if the Wanderer has reached the current patrol point
        if (Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.1f)
        {
            // Wait for the specified wait time before moving to the next patrol point
            StartCoroutine(WaitAtPatrolPoint());
        }
        else
        {
            // Move towards the current patrol point
            Vector3 direction = (patrolPoints[currentPointIndex].position - transform.position).normalized;
            Move(direction);
        }
    }

    private System.Collections.IEnumerator WaitAtPatrolPoint()
    {
        // Wait for the specified wait time
        yield return new WaitForSeconds(waitTime);

        // Move to the next patrol point
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    public void Morph()
    {
        // Implement Morph logic here (e.g., change appearance)
    }
}
