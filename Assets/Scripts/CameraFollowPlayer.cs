using UnityEngine;

/// <summary>
/// Controls the camera to follow the player smoothly in a third-person perspective.
/// A little bumpy will mess aroudn with the smooth values
/// </summary>
public class CameraFollowPlayer : MonoBehaviour
{
    // The speed the camera follows the player. 
    public float smoothSpeed = 0.125f;

    // The player position offset
    public Vector3 offset = new Vector3(0, 5, -10);
    
    private Transform target;

    /// <summary>
    /// LateUpdate is called after all Update functions have been called according to documentation image
    /// Using here so that the camera moves after the player has moved.
    /// </summary>
    private void LateUpdate()
    {
        // If can't find a target (player), try to find one
        if (target == null)
        {
            FindPlayer();
            return;
        }

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + offset;

        // Interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;
        
        transform.LookAt(target);
    }

    /// <summary>
    /// Find player using "Player" tag
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