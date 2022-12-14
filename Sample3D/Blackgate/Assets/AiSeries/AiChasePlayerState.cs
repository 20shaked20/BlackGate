using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    float timer = 0.0f;   
    public void Enter(AiAgent agent)
    {
        Debug.Log("Agent Chase Player");
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;

            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }
        

        /*if player ran from agent, change the state to patrol*/
        // if(!agent.sensor.IsInSight(agent.playerTransform.gameObject))
        // {
        //     agent.navMeshAgent.speed = 0.2659392f;
        //     agent.stateMachine.ChangeState(AiStateId.Patrol);
        // }
        /*if agent reached player, start attacking him*/
        if(agent.config.CheckIfPlayerInAttackRange(agent))
        {
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }
 
}
