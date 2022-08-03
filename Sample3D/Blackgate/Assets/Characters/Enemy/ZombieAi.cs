using UnityEngine;
using UnityEngine.AI;

public class ZombieAi : MonoBehaviour
{   

    [Header("Zombie")]
    [Tooltip("Move speed of the character in m/s")]
    public float Idle = 0f;
    public float WalkSpeed = 2f;
    public float RunSpeed = 6f;
    public float SpeedChangeRate = 10.0f;

    private float _animationBlend;
    
    [Header("Objects")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator _animator;
    

    public float health;


    /*Patrolling*/
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
 
    /*Attacking*/
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    /*States*/
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool zombieDead;

    /*test*/
    // [Header("TEST methods")]
    // public ThirdPersonShooterController tps;
    // public float fov = 120f;
    // public float viewDistance = 10f;
    // private bool isAware = false;
    // public void SearchForPlayer()
    // {
    //     if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(tps.transform.position)) < fov / 2f)
    //     {
    //         if(Vector3.Distance(tps.transform.position, transform.position) < viewDistance)
    //         {
    //             OnAware();
    //         }
    //     }
    // }
    // public void OnAware()
    // {
    //     isAware = true;
    // }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        player = GameObject.Find("Akai").transform; /*player gameobject name*/
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!zombieDead)
        {

            /*check for sight and attack range*/
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); /*SearchForPlayer();*/
            
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange/*isAware*/) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
            
        }

    }

    private void Patroling()
    {   
        // Debug.Log("Zombie Patrolling");
        _animator.SetBool("IsAttacking", false);/*not attacking now*/

        _animationBlend = Mathf.Lerp(_animationBlend, WalkSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        _animator.SetFloat("Speed", _animationBlend);/*set zombie to walk*/

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {   
            agent.speed = 0.5f; /*walk speed*/
            agent.SetDestination(walkPoint); /*the nav mesh object knows by it self how to navigate!*/
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        /*if distance is less than 1, we've reached the walk point, 
        and then set to false to automaticly search for a new one*/
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        /*Calculate random point in range to walk*/
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        

        walkPoint = new Vector3(x: transform.position.x + randomX, y: transform.position.y, z: transform.position.z + randomZ);
        Debug.Log("WalkPoint :"+ walkPoint);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) /*bugging the ai rn*/
            walkPointSet = true;
    }

    private void ChasePlayer()
    {   
        _animator.SetBool("IsAttacking", false);/*not attacking now*/

         _animationBlend = Mathf.Lerp(_animationBlend, RunSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        _animator.SetFloat("Speed",_animationBlend);/*chasing player im changing the zombie to run*/

        agent.speed = 8f; /*run speed*/
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        /*Make sure enemy doesn't move*/
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            /*in here need to add what type of attack*/
            _animator.SetBool("IsAttacking", true);

            /**/
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); /*gap between each attack to reset it*/
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }

    }

    private void DestroyEnemy()
    {
        _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(0), 0f, Time.deltaTime * 1f)); /*cahnge layer to zombie dying*/
        _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f)); /*cahnge layer to zombie dying*/
        zombieDead = true;
        _animator.SetBool("IsDead",true);

        Destroy(agent);
        // Destroy(gameObject);
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
