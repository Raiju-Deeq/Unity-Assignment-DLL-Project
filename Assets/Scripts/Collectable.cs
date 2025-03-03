using UnityEngine;
using GameplayPlugin;

public class Collectable : MonoBehaviour
{
    public int scorePoints = 10;
    public PowerUpEffect.PowerUpType powerUpType; // Set this in the Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null) // Check if the collider belongs to the player
        {
            // Access the GameManager and add the score
            GameManager.Instance?.AddScore(scorePoints); // Use null-conditional operator

            // Access the PowerUpEffect script and apply
            PowerUpEffect? powerUpEffect = GetComponent<PowerUpEffect>(); // Use null-conditional operator
            if (powerUpEffect != null)
            {
                powerUpEffect.powerUpType = powerUpType; // Set the power-up type
                powerUpEffect.ApplyEffect();
            }

            // Destroy the collectable
            Destroy(gameObject);
        }
    }
}
