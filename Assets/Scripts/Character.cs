using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for all characters in the game.
/// </summary>
public abstract class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;  // The name of the character
    [SerializeField] protected float movementSpeed;   // The movement speed of the character
    [SerializeField] protected float damage;          // The damage dealt by the character
    [SerializeField] protected float border = 24.5f;  // The boundary limit for character movement

    /// <summary>
    /// Returns the name of the character.
    /// </summary>
    /// <returns>The character's name as a string.</returns>
    public string GetName() => characterName;

    /// <summary>
    /// Clamps the character's position within the defined border, labs logic
    /// </summary>
    protected void ClampPosition()
    {
        Vector3 borderclamp = transform.position;
        // Clamp the x position between -border and border
        borderclamp.x = Mathf.Clamp(borderclamp.x, -border, border);
        // Clamp the z position between -border and border
        borderclamp.z = Mathf.Clamp(borderclamp.z, -border, border);
        // Update the character's position with the clamped values
        transform.position = borderclamp;
    }
}