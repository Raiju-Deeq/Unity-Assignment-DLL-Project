using System.Collections.Generic;
using UnityEngine;
using GamePlayDLL;
//This script manages the application and duration of power-ups for the player, including their effects and expiration.
public class PowerUpManager
{
    private Dictionary<PowerUpType, float> activePowerUps = new Dictionary<PowerUpType, float>();

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
            // Implement logic to remove power-up effect
        }
    }
}