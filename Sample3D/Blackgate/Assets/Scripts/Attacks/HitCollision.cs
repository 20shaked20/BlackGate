using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour
{
    public GameObject bloodParticle;
    GameObject bloodSpray;

    private void OnParticleCollision(GameObject other)
    {   
        ZombieAi z;
        if( z = other.GetComponent<ZombieAi>())
        {
            z.TakeDamage(10);
            bloodSpray = Instantiate(bloodParticle, transform.position, Quaternion.identity);
            bloodSpray.transform.parent = z.transform; /*set the blood particle as child of the zombie so it will stick to him*/
            Destroy(gameObject); /*does not need to see the hit effect on walls when attacking zombies*/
        }
        
    }
    
}
