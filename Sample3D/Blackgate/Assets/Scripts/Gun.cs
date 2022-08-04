using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*this is a script for the gun, it handles flash,muzzzle etc..
TODO: add animator*/

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    [SerializeField] bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f,0.1f,0.1f);
    [SerializeField] private ParticleSystem ShootingSystem;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticaleSystem;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private LayerMask Mask;
    public float Damage = 10; 

    // private Animator Animator;
    private float LastShootTime;

    void Awake()
    {
        // Animator = GetComponent<Animator>();
    }
    
    public void Shoot(Vector3 aimDir)
    {
        if(LastShootTime + ShootDelay < Time.time)
        {
            /*use an object pool instead for these! not happening in toturial*/
            // Animator.SetBool("IsShooting",true);
            // ShootingSystem.Play();

            if(Physics.Raycast(BulletSpawnPoint.position, aimDir, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail,hit));

                LastShootTime = Time.time;
            }

            /*test*/
            var hitBox = hit.collider.GetComponent<HitBox>();
            if(hitBox)
                hitBox.OnRaycastHit(this, aimDir);
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while(time < 1 )
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }

        // Animator.SetBool("IsShooting",false);
        Trail.transform.position = Hit.point;
        // if(Hit.transform.tag != "Enemy")
            // Instantiate(ImpactParticaleSystem, Hit.point, Quaternion.LookRotation(Hit.normal)); /*removed for now, because is bugged with enemy hit*/

        Destroy(Trail.gameObject, Trail.time);
    }
}
