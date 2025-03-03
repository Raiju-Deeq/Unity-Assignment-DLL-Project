using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform? target;         // The target to follow (the player)
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Offset from the target
    public float smoothSpeed = 0.125f;  // Smoothing factor for camera movement
    public Vector3 positionOffset; // Offset from the target

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned to CameraFollow script.");
            return;
        }

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position + offset + positionOffset;

        // Smoothly move the camera to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the target
        transform.LookAt(target);
    }
}
