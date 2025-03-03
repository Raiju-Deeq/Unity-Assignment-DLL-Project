using UnityEngine;
using GameplayPlugin; // Use the DLL's namespace

public class PlayerMovement : MonoBehaviour, IMovement
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f; // Adjust the rotation speed as needed
    public float MoveSpeed // Implement the interface property
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private CharacterController? characterController; // Use CharacterController

    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController not found on Player.");
            enabled = false; // Disable the script if CharacterController is missing
        }
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Get input for horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotate the player to face the movement direction
        if (moveDirection.magnitude > 0.1f) // Only rotate if there is significant movement
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move the player using CharacterController
        if (characterController != null)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
