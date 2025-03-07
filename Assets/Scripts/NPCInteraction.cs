using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles interaction with an NPC that displays game controls.
/// Currently, disabled due to issues with player OnTrigger handling.
/// </summary>
public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private string dialogue = "Press E to view game controls.";
    [SerializeField] private GameObject controlsUI; 
    [SerializeField] private float interactionRange;

    private bool isPlayerNearby = false;

    /// <summary>
    /// Checks for player input to show controls when nearby.
    /// </summary>
    private void Update()
    {
        // Check for interaction input
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ShowControls();
        }
    }

    /// <summary>
    /// Displays the controls UI and logs the dialogue.
    /// </summary>
    private void ShowControls()
    {
        if (controlsUI != null)
        {
            controlsUI.SetActive(true); // Activate the UI panel
            Debug.Log(dialogue); // Log dialogue for debugging
        }
        else
        {
            Debug.LogWarning("Controls UI is not assigned!");
        }
    }

    /// <summary>
    /// Detects when the player enters the interaction range.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the interaction range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    /// <summary>
    /// Detects when the player leaves the interaction range and hides the UI.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the interaction range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (controlsUI != null)
            {
                controlsUI.SetActive(false); // Hide the UI when player moves away
            }
        }
    }
}
