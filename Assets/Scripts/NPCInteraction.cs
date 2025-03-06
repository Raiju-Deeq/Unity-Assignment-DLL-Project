using UnityEngine;
using UnityEngine.UI;
// Tutorial NPC currently not working properly with how player handles ontrigger
// Disabled NPC object for now come back for testing and fixing later

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private string dialogue = "Press E to view game controls.";
    [SerializeField] private GameObject controlsUI; // Reference to the UI panel displaying controls
    [SerializeField] private float interactionRange; // Distance within which the player can interact

    private bool isPlayerNearby = false;

    private void Update()
    {
        // Check for interaction input
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ShowControls();
        }
    }

    private void ShowControls()
    {
        if (controlsUI != null)
        {
            controlsUI.SetActive(true); // Activate the UI panel
            Debug.Log(dialogue); // Optionally log dialogue for debugging
        }
        else
        {
            Debug.LogWarning("Controls UI is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the interaction range
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

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