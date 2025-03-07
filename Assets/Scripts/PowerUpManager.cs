using System.Collections.Generic;
using UnityEngine;
using GamePlayDLL;

/// <summary>
/// Manages the application, duration, and removal of power-ups for the player.
/// Handles different types of power-ups and their effects.
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    // Dictionary to store active power-ups
    private Dictionary<PowerUpType, float> activePowerUps = new Dictionary<PowerUpType, float>();

    /// <summary>
    /// Applies a power-up to the player or updates its duration if already active.
    /// </summary>
    /// <param name="player">The player to apply the power-up to.</param>
    /// <param name="type">The type of power-up to apply.</param>
    /// <param name="duration">The duration of the power-up in seconds.</param>
    public void ApplyPowerUp(IPlayer player, PowerUpType type, float duration)
    {
        // Apply or update power-up duration
        if (activePowerUps.ContainsKey(type))
        {
            activePowerUps[type] = duration;
        }
        else
        {
            activePowerUps.Add(type, duration);
        }

        ApplyPowerUpEffect(player, type);
    }

    /// <summary>
    /// Applies the specific effect of a power-up to the player.
    /// </summary>
    /// <param name="player">The player to apply the effect to.</param>
    /// <param name="type">The type of power-up effect to apply.</param>
    private void ApplyPowerUpEffect(IPlayer player, PowerUpType type)
    {
        // Apply specific power-up effects
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                // Implement speed boost logic
                break;
            case PowerUpType.Shield:
                // Implement shield logic
                break;
            case PowerUpType.DoublePoints:
                // Implement double points logic
                break;
        }
    }

    /// <summary>
    /// Updates the duration of active power-ups and removes expired ones.
    /// </summary>
    public void UpdatePowerUps()
    {
        // Update power-up durations and remove expired ones
        List<PowerUpType> expiredPowerUps = new List<PowerUpType>();
        foreach (var powerUp in activePowerUps)
        {
            activePowerUps[powerUp.Key] -= Time.deltaTime;
            if (activePowerUps[powerUp.Key] <= 0)
            {
                expiredPowerUps.Add(powerUp.Key);
            }
        }

        foreach (var expiredPowerUp in expiredPowerUps)
        {
            activePowerUps.Remove(expiredPowerUp);
            // TODO: Implement logic to remove power-up effect from the player 
        }
    }
}
