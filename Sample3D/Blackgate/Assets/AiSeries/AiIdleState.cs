using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public void Enter(AiAgent agent)
    {
        Debug.Log("Agent idle");
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }

    public void Update(AiAgent agent)
    {
        agent.stateMachine.ChangeState(AiStateId.Patrol);
    }
}
