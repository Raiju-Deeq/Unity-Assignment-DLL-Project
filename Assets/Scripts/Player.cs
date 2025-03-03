using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private float maximumSpeed = 10f; // Example of a specific property
    [SerializeField]
    private int healthCount = 3;

    //Reference the SpawningManager to access the methods
    private GameplayPlugin.SpawningManager spawningManager;

    void Start()
    {
        spawningManager = FindObjectOfType<GameplayPlugin.SpawningManager>();

        if (spawningManager == null)
        {
            Debug.LogError("SpawningManager not found in the scene.");
        }
    }
    public void MovePlayer(Vector3 direction)
    {
        // Can add player-specific movement logic here, like limiting speed
        base.Move(direction); // Calls the base class's Move method
        movementSpeed = Mathf.Clamp(movementSpeed, 0, maximumSpeed);
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        // Implement Die logic here
    }

    public void Respawn()
    {
        Debug.Log("Player has respawned!");
        // Implement Respawn logic here
    }
}
