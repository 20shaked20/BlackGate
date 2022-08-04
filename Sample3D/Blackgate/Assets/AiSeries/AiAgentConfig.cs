using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float dieForce = 10.0f;
    public float maxSightDistance = 5.0f;
    public float attackRange = 0.6f;
    public float walkPointRange = 3f;
    public float viewAngle = 85.0f;
    public LayerMask PlayerLayerMask;


    public bool CheckIfPlayerInSight(AiAgent agent)
    {
        Vector3 enemyDir = agent.playerTransform.position - agent.transform.position;

        float angle = Vector3.Angle(agent.transform.forward, enemyDir);
        Debug.Log(angle);

        if (angle > 0.0f && angle < viewAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfPlayerInAttackRange(AiAgent agent)
    {
        return Physics.CheckSphere(agent.transform.position, attackRange, agent.config.PlayerLayerMask);
    }
}
