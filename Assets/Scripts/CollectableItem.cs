using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlayDLL;
using Managers;
using UML;


/// <summary>
/// Represents a collectable item in the game.
/// Handles interactions with objects implementing IPlayer.
/// </summary>
public class CollectableItem : MonoBehaviour
{
    private ICollectable collectableEffect; // Reference to the collectable effect

    private void Start()
    {
        if (gameObject.CompareTag("SpeedBoost"))
            collectableEffect = new SpeedBoost();
        else if (gameObject.CompareTag("Shield"))
            collectableEffect = new Shield();
        else
            Debug.LogWarning("Unknown collectable type!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPlayer>(out IPlayer player))
        {
            if (collectableEffect != null)
            {
                collectableEffect.ApplyEffect(player); // Apply effect using DLL's interface method

                FindObjectOfType<UIManager>().AddScore(collectableEffect.GetScoreValue());
                Debug.Log($"Collectable applied: {collectableEffect.GetScoreValue()} points.");

                Destroy(gameObject); // Destroy collectable after use
            }
            else
            {
                Debug.LogWarning("No collectable effect assigned!");
            }
        }
        else
        {
            Debug.LogWarning("Collided object does not implement IPlayer!");
        }
    }
}