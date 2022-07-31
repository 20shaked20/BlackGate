using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    /*Patrolling*/
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    /*Attacking*/
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    /*States*/
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Akai").transform; /*player gameobject name*/
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        /*check for sight and attack range*/
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    private  void Patroling()
    {
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint); /*the nav mesh object knows by it self how to navigate!*/
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        /*if distance is less than 1, we've reached the walk point, 
        and then set to false to automaticly search for a new one*/
        if(distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        /*Calculate random point in range to walk*/
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(x: transform.position.x + randomX, y:transform.position.y , z: transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        /*Make sure enemy doesn't move*/
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            /*in here need to add what type of attack*/
            Rigidbody rb = Instantiate(projectile,transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            /**/
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks); /*gap between each attack to reset it*/
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0) Invoke(nameof(DestroyEnemy),0.5f);
        
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    /*visualise attack and sight range method*/
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
