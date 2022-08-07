using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackState : AiState
{
    bool alreadyAttacked;
    public void Enter(AiAgent agent)
    {
        Debug.Log("Agent attack");
    }

    public void Exit(AiAgent agent)
    {
        agent.animator.SetBool("IsAttacking", false);
    }

    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Update(AiAgent agent)
    {
        /*Make sure enemy doesn't move*/
        agent.navMeshAgent.SetDestination(agent.transform.position);

        agent.transform.LookAt(agent.playerTransform);

        /*checking if player in attack range & can attack */
        if (!alreadyAttacked && agent.config.CheckIfPlayerInAttackRange(agent))
        {
            /*in here need to add what type of attack*/
            agent.animator.SetBool("IsAttacking", true);

            /**/
            alreadyAttacked = true;
            ResetAttack(); /*gap between each attack to reset it*/
        }

        /*case where player is still in sight but ran from agent so agent will chase him*/
        if(!agent.config.CheckIfPlayerInAttackRange(agent) && agent.sensor.IsInSight(agent.playerTransform.gameObject))
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
        /*case where player is not in attack range and not in sight, simple revert to patrol*/
        // else if(!agent.config.CheckIfPlayerInAttackRange(agent) && !agent.sensor.IsInSight(agent.playerTransform.gameObject))
        // {
        //      agent.navMeshAgent.speed = 0.2659392f;
        //      agent.stateMachine.ChangeState(AiStateId.Patrol);
        // }
        
    }
    private void ResetAttack()
    {
        new WaitForSeconds(0.1f);
        alreadyAttacked = false;
    }

}
