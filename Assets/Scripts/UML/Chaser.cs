using UnityEngine;
namespace UML
{
    public class Chaser : Enemy
    {
        [SerializeField] private float retreatDistance = 5f;

        private void Update()
        {
            if (enemyState == EnemyState.Chasing)
                ChasePlayer();
            else if (enemyState == EnemyState.Retreating)
                Retreat();
        
            Move();
        }

        public void ChasePlayer()
        {
            if (target == null) return;

            transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
            Debug.Log($"{GetName()} is chasing the player!");
        
            if (Vector3.Distance(transform.position, target.position) < retreatDistance)
                enemyState = EnemyState.Retreating;
        }

        public void Retreat()
        {
            if (target == null) return;

            transform.position = Vector3.MoveTowards(transform.position, -target.position, movementSpeed * Time.deltaTime);
            Debug.Log($"{GetName()} is retreating!");
        
            if (Vector3.Distance(transform.position, target.position) > retreatDistance * 2)
                enemyState = EnemyState.Idle;
        }

        protected override void Move()
        {
            // Movement logic is handled in ChasePlayer() and Retreat().
            // This method can remain empty unless additional behavior is needed.
        }
    }
}