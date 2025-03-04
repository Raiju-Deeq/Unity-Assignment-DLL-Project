using UnityEngine;
namespace UML
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private string characterName;
        [SerializeField] protected float movementSpeed;

        /// <summary>
        /// Gets the name of the character.
        /// </summary>
        /// <returns>The character's name.</returns>
        public string GetName()
        {
            return characterName;
        }

        /// <summary>
        /// Moves the character (to be implemented by derived classes).
        /// </summary>
        protected abstract void Move();
    }
}