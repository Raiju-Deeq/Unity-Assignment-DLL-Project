using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This script defines an abstract base class for all characters in the game. It includes common properties like name and movement speed, and a method to get the character's name.
public abstract class Character : MonoBehaviour
{
    // Protected variables accessible to derived classes
    [SerializeField] protected string characterName;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float damage;
    [SerializeField] protected float border = 24.5f;

    // Public method to get the character's name
    public string GetName() => characterName;
    
    protected void ClampPosition()
    {
        Vector3 borderclamp = transform.position;
        borderclamp.x = Mathf.Clamp(borderclamp.x, -border, border); // Clamp the x position
        borderclamp.z = Mathf.Clamp(borderclamp.z, -border, border); // Clamp the z position
        transform.position = borderclamp; // Update the position
    }
}