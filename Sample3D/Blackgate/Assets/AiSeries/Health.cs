using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    
    public GameObject bloodParticle;

    [HideInInspector]
    public float currentHealth;
    
    [HideInInspector]
    AiAgent agent;

    void Start()
    {
        agent = GetComponent<AiAgent>();
        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            hitBox.bloodParticle = bloodParticle;
        }
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }
    }

    private void Die(Vector3 direction)
    {
       AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
       deathState.direction = direction;
       agent.stateMachine.ChangeState(AiStateId.Death);
    }
}
