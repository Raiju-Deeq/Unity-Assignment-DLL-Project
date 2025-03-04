using UnityEngine;
namespace UML
{
    public abstract class Enemy : Character
    {
        public enum EnemyState { Idle, Patrolling, Chasing, Retreating }
    
        [SerializeField] protected EnemyState enemyState;
        [SerializeField] protected internal Transform target;

        /// <summary>
        /// Gets the current state of the enemy.
        /// </summary>
        public EnemyState GetEnemyState()
        {
            return enemyState;
        }

        /// <summary>
        /// Destroys the enemy object.
        /// </summary>
        public void Destroy()
        {
            Debug.Log($"{GetName()} has been destroyed!");
            Destroy(gameObject);
        }

        protected override abstract void Move();
    }
}