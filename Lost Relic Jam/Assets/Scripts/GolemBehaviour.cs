using UnityEngine;
using UnityEngine.AI;

namespace Golems{
    public class GolemBehaviour : MonoBehaviour, IGolemBehaviour
    {
        public NavMeshAgent agent; 

        public bool isPlayerInAttackRange {get; private set;}
        public bool isPlayerNoticeable { get; private set;}
        public bool isAlive {get; private set;}
        public bool hasAttacked {get; private set;}

        public float timeBetweenAttacks {get; private set;}

        public LayerMask whatIsPlayer {get; private set;}
        public LayerMask whatIsGround {get; private set;}

        public Transform player {get; private set;}

        // This serrves as a temporary fast fix to player object, the ideal case would be to change it on line 45
        // as it ask for an String referring to the objects name and not the Object reference itself
        public string playerObject;

        public Vector3 walkPoint;
        bool walkCheckPoint;
        public float walkPointRange;

        // This serves as a controller for the hit logic, if the player is hitten 3 times this will kill it;
        //[HideInInspector]
        public int counter; //If the player has 0 velocity when the monster attacks it would be instakill.
        [SerializeField]
        float timer;

        // this two values will represent the distance where the player can be noticed and the distance for being attacked
        public float noticeRange, attackRange;

        //[SerializeField]
        //private Animator golemAnim = null;

        private void Awake() {

            player = GameObject.Find(playerObject).transform;
            agent = GetComponent<NavMeshAgent>();
            //golemAnim = GetComponent<Animator>();
            

        }

        void Start() {

            whatIsPlayer = LayerMask.GetMask("Player");
            whatIsGround = LayerMask.GetMask("Ground");
            //player = GameObject.Find(playerObject).transform.position;

        }

        void Update() {

            isPlayerNoticeable = Physics.CheckSphere(transform.position, noticeRange, whatIsPlayer);
            isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if(!isPlayerNoticeable && !isPlayerInAttackRange) PatrolState();
            if(isPlayerNoticeable && !isPlayerInAttackRange) ChasingState();
            if(isPlayerNoticeable && isPlayerInAttackRange) AttackState();
        }

        private void PatrolState() {
            Debug.Log("Im patroling");
            if (!walkCheckPoint) {
                Debug.Log("Should be searching for a random point");
                SearchForWalkPoint();
            }
            if (walkCheckPoint) 
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
            Debug.Log("Im chasing the player");
            agent.SetDestination(player.position);

        }

        private void AttackState() {
            Debug.Log("Attack state entered");
            
            // i don't want the golem to continue it's movement since it's supposed to stop if it's going to attack the player
            agent.SetDestination(transform.position);
            
            transform.LookAt(player);

            if (!hasAttacked) {
                hasAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                AttackCounter();
            }
        }

        private void ResetAttack() {
            Debug.Log("Should be reseting the attack");
            hasAttacked = false;
        }

        private void AttackCounter() {
            counter++;

            if (counter == 1) {
                Debug.Log("The first attack ahs been made");
                // add rb.force to the player
            } 
            else if(counter == 2) {
                Debug.Log("Second attack incoming");
                // remove health from the player
            }
            else if(counter == 3) {
                Debug.Log("U dead time for despawn");
                timer += Time.deltaTime; // This timer only works for definig how many time it has to wait before destroying the object
                //if(timer  >= 5) destroy(Barbarian)  
            } //else if(counter == 1 && playerVelocity == 0f){ destroy(Barbarian)}
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, noticeRange);
        }
    }
}