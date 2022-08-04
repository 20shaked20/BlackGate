using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrollState : AiState
{
    public Vector3 walkPoint;
    bool walkPointSet;
    
    public void Enter(AiAgent agent)
    {
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Patrol;
    }

    public void Update(AiAgent agent)
    {
        if (!walkPointSet) SearchWalkPoint(agent);

        if (walkPointSet)
        {
            agent.navMeshAgent.speed = 0.2659392f; /*walk speed(its according to the blend tree)*/
            agent.navMeshAgent.SetDestination(walkPoint); /*the nav mesh object knows by it self how to navigate!*/
        }

        Vector3 distanceToWalkPoint = agent.transform.position - walkPoint;

        /*if distance is less than 1, we've reached the walk point, 
        and then set to false to automaticly search for a new one*/
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;


        if(agent.config.CheckIfPlayerInSight(agent))
        {
            agent.navMeshAgent.speed = 3;
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

    }

    private void SearchWalkPoint(AiAgent agent)
    {
        /*Calculate random point in range to walk*/
        float randomZ = Random.Range(-agent.config.walkPointRange, agent.config.walkPointRange);
        float randomX = Random.Range(-agent.config.walkPointRange, agent.config.walkPointRange);


        walkPoint = new Vector3(x: agent.transform.position.x + randomX, y: agent.transform.position.y, z: agent.transform.position.z + randomZ);
        Debug.Log("WalkPoint :" + walkPoint);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) /*bugging the ai rn*/
        walkPointSet = true;
    }
}
