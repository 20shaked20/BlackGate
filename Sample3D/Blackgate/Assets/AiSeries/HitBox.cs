using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    public GameObject bloodParticle;
    GameObject bloodSpray;
    public void OnRaycastHit(Gun weapon, Vector3 direction)
    {
        health.TakeDamage(weapon.Damage, direction);
        bloodSpray = Instantiate(bloodParticle, transform.position, Quaternion.identity);
        bloodSpray.transform.parent = health.transform; /*putting the blood spray as child of the ragdoll objects*/
        
    }

}
