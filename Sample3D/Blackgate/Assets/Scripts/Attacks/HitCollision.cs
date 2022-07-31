using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour
{
    
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.TryGetComponent<ZombieAi>(out ZombieAi enemyComponenet))
    //     {
    //         enemyComponenet.TakeDamage(10);
    //     }
    //     Destroy(gameObject);
    // }
    private void OnParticleCollision(GameObject other)
    {   
        ZombieAi z;
        if( z = other.GetComponent<ZombieAi>())
            z.TakeDamage(10);
    }
    
}
