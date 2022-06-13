using UnityEngine;
using UnityEngine.AI;

namespace Golems{
    public class GolemBehaviour : MonoBehaviour
    {
        public NavMeshAgent agent; 

        public Transform player;

        public LayerMask whatIsPlayer, whatIsGround;

        public Vector3 walkPoint;
        bool walkCheckPoint;
        public float walkPointRange;

        // This serves as a controller for the hit logic, if the player is hitten 3 times this will kill it;
        [SerializeField]
        public int counter; //If the player has 0 velocity when the monster attacks it would be instakill.
        [SerializeField]
        float timer;

        // this two values will represent the distance where the player can be noticed and the distance for being attacked
        public float noticeRange, attackRange;
        public bool isPlayerIsNoticeable, isPlayerInAttackRange;
        bool hasAttacked;

        private void Awake() {

            //player = GameObject.Find("Barbarian").transform;
            agent.GetComponent<NavMeshAgent>();

        }

        void Update() {

            isPlayerIsNoticeable = Physics.CheckSphere(transform.position, noticeRange, whatIsPlayer);
            isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if(!isPlayerIsNoticeable && !isPlayerInAttackRange) PatrolState();
            if(isPlayerIsNoticeable && !isPlayerInAttackRange) ChasingState();
            if(isPlayerIsNoticeable && isPlayerInAttackRange) AttackState();
        }

        private void PatrolState() {
            if (!walkCheckPoint) 
                SearchForWalkPoint();
            else 
                agent.SetDestination(walkPoint);
            
            Vector3 distanceToPoint = transform.position - walkPoint;

            if (distanceToPoint.magnitude < 1f) walkCheckPoint = false;

        }

        private void SearchForWalkPoint() {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
                walkCheckPoint = true;
            }

        }

        private void ChasingState() {

            agent.SetDestination(player.position);

        }

        private void AttackState() {
            Debug.Log("Attack state entered");
            // i don't want the golem to continue it's movement since it's supposed to stop if it's going to attack the player
            agent.SetDestination(transform.position);
            

            // See if this doesn't make the golem to rotate on itself if the player moves while attacking
            transform.LookAt(player);

            if (!hasAttacked) {
                hasAttacked = true;
                Invoke(nameof(ResetAttack), 10f);
                AttackCounter();
            }
        }

        private void ResetAttack() {
            hasAttacked = false;
        }

        private void AttackCounter() {
            counter++;

            if (counter == 1) {
                // add rb.force to the player
            } 
            else if(counter == 2) {
                // remove health from the player
            }
            else if(counter == 3) {
                
                timer += Time.deltaTime;
                //if(timer  >= 5) destroy(Barbarian)  
            } //else if(counter == 1 && playerVelocity == 0f){ destroy(Barbarian)}
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")) AttackState();
            
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, noticeRange);
        }
    }
}