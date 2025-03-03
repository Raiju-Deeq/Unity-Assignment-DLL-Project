using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected string characterName = "DefaultName"; // Encapsulation
    [SerializeField]
    protected float movementSpeed = 5f;

    public virtual string GetName()
    {
        return characterName;
    }

    // Example of a virtual method that can be overridden in derived classes
    public virtual void Move(Vector3 direction)
    {
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }
}
