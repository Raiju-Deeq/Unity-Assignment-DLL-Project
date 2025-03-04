using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamePlayDLL;
// This script defines the Wanderer enemy type, which moves between randomly generated patrol points within a defined area.
public class Wanderer : MonoBehaviour, IEnemy
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] internal Vector3 spawnAreaMin; // Minimum bounds of spawn area
    [SerializeField] internal Vector3 spawnAreaMax; // Maximum bounds of spawn area
    [SerializeField] private int numberOfPatrolPoints = 5; // Number of patrol points to generate

    private List<Vector3> patrolPoints; // List to store generated patrol points
    private int currentPatrolIndex;

    private void Start()
    {
        GenerateRandomPatrolPoints();
        StartCoroutine(PatrolRoutine());
    }

    private void GenerateRandomPatrolPoints()
    {
        // Generate random patrol points within the spawn area
        patrolPoints = new List<Vector3>();
        for (int i = 0; i < numberOfPatrolPoints; i++)
        {
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
            Vector3 randomPoint = new Vector3(randomX, randomY, randomZ);
            patrolPoints.Add(randomPoint);
        }
        Debug.Log($"Generated {patrolPoints.Count} random patrol points.");
    }

    private IEnumerator PatrolRoutine()
    {
        // Continuously patrol between generated points
        while (true)
        {
            if (patrolPoints == null || patrolPoints.Count == 0) yield break;
            Vector3 targetPosition = patrolPoints[currentPatrolIndex];
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }

    public void Attack(IPlayer player)
    {
        player.TakeDamage(5);
    }

    public int GetScoreValue() => 30;
}
