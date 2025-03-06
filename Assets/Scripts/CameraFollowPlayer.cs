using UnityEngine;

/// <summary>
/// Controls the camera to follow the player smoothly in a third-person perspective.
/// This script should be attached to the main camera in the scene.
/// </summary>
public class CameraFollowPlayer : MonoBehaviour
{
    // The speed at which the camera follows the player. Lower values make the camera more sluggish.
    public float smoothSpeed = 0.125f;

    // The offset from the player's position. Adjust these values to change the camera's position relative to the player.
    public Vector3 offset = new Vector3(0, 5, -10);

    // Reference to the player's transform component
    private Transform target;

    /// <summary>
    /// LateUpdate is called after all Update functions have been called.
    /// This ensures that the camera moves after the player has moved.
    /// </summary>
    private void LateUpdate()
    {
        // If we don't have a target (player), try to find one
        if (target == null)
        {
            FindPlayer();
            return;
        }

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Make the camera look at the player
        transform.LookAt(target);
    }

    /// <summary>
    /// Finds the player in the scene using the "Player" tag.
    /// </summary>
    private void FindPlayer()
    {
        // Look for a GameObject with the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // If a player is found, set it as our target
        if (player != null)
        {
            target = player.transform;
        }
    }
}