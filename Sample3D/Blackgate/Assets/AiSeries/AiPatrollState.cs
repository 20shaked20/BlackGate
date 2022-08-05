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


        /*if player is in agent sight, chase him*/
        if(agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            Debug.Log("Player In Sight");
            agent.navMeshAgent.speed = 3;
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

    }

    private void SearchWalkPoint(AiAgent agent)
    {
        /*Calculate random point in range to walk*/

        WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
        Vector3 min = worldBounds.min.position;
        Vector3 max = worldBounds.max.position;

        walkPoint = new Vector3(
            Random.Range(min.x,max.x),
            Random.Range(min.y,max.y),
            Random.Range(min.z,max.z)
        );
        Debug.Log("WalkPoint :" + walkPoint);

        walkPointSet = true;
    }
}
