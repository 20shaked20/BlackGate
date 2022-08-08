using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem BuildingImpactParticleSystem;
    [SerializeField] private ParticleSystem WaterImpactParticleSytem;

    private void OnParticleCollision(GameObject other)
    {

        if (other.tag == "Building")
        {

            Instantiate(BuildingImpactParticleSystem, transform.position, Quaternion.identity);
        }
        if (other.tag == "Water")
        {

            // Instantiate(BuildingImpactParticleSystem, transform.position, Quaternion.identity);
        }
    }

}
