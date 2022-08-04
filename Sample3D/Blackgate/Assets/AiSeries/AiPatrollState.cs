using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrollState : AiState
{
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 3f;

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


        if(CheckIfPlayerInSight(agent))
        {
            agent.navMeshAgent.speed = 3;
        }

    }

    private void SearchWalkPoint(AiAgent agent)
    {
        /*Calculate random point in range to walk*/
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);


        walkPoint = new Vector3(x: agent.transform.position.x + randomX, y: agent.transform.position.y, z: agent.transform.position.z + randomZ);
        Debug.Log("WalkPoint :" + walkPoint);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) /*bugging the ai rn*/
        walkPointSet = true;
    }

    private bool CheckIfPlayerInSight(AiAgent agent)
    {
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        if (playerDirection.magnitude < agent.config.maxSightDistance)
        {
            return false;
        }

        Vector3 agentDirection = agent.transform.forward;

        playerDirection.Normalize();

        /*if agent is facing the player, then change state and chase him*/
        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (dotProduct > 0.0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
        return true;
    }
}
