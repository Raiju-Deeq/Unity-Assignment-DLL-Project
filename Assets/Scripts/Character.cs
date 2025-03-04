using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This script defines an abstract base class for all characters in the game. It includes common properties like name and movement speed, and a method to get the character's name.
public abstract class Character : MonoBehaviour
{
    // Protected variables accessible to derived classes
    [SerializeField] protected string characterName;
    [SerializeField] protected float movementSpeed;

    // Public method to get the character's name
    public string GetName() => characterName;
}