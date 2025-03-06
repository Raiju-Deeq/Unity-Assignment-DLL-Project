using UnityEngine;
using System.Collections;

/// <summary>
/// Represents an environmental hazard that can damage the player. 
/// The hazard alternates between active and inactive states.
/// Right now kills the player if player collides with it atfer the hazard transitions from active to inactive states
/// </summary>
public class EnvironmentHazard : MonoBehaviour
{
    /// <summary>
    /// The amount of damage dealt to the player when the hazard is active.
    /// </summary>
    //public int damageAmount = 10;

    /// <summary>
    /// The time interval between state changes (active/inactive).
    /// </summary>
    public float activationInterval = 2f;

    /// <summary>
    /// The color of the hazard when it's in the active state.
    /// </summary>
    public Color activeColor = Color.red;

    /// <summary>
    /// The color of the hazard when it's in the inactive state.
    /// </summary>
    public Color inactiveColor = Color.gray;

    private bool isActive = false;
    private Renderer hazardRenderer;

    /// <summary>
    /// Initializes the hazard and starts the activation cycle.
    /// </summary>
    private void Start()
    {
        hazardRenderer = GetComponent<Renderer>();
        StartCoroutine(ActivationCycle());
    }

    /// <summary>
    /// Coroutine that continuously alternates the hazard's state.
    /// </summary>
    private IEnumerator ActivationCycle()
    {
        while (true)
        {
            isActive = !isActive;
            UpdateVisuals();
            PlayStateChangeSound();
            yield return new WaitForSeconds(activationInterval);
        }
    }

    /// <summary>
    /// Updates the visual appearance of the hazard based on its current state.
    /// </summary>
    private void UpdateVisuals()
    {
        hazardRenderer.material.color = isActive ? activeColor : inactiveColor;
    }

    /// <summary>
    /// Plays a sound effect when the hazard changes state.
    /// </summary>
    private void PlayStateChangeSound()
    {
        // Play appropriate sound effect (implement this later don't have time right now)
    }

    /// <summary>
    /// Damages the player if they are in contact with the hazard while it's active.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                //player.TakeDamage(damageAmount);
                player.Die();
            }
        }
    }
}
