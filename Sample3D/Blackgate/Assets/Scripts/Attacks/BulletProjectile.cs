using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class BulletProjectile : MonoBehaviour
{   
    // [SerializeField] private Transform vfxHit;/*particale on hit*/
    [SerializeField] private GameObject vfxHit;
    private Rigidbody bulletRigidbody;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        // vfxHit = GetComponent<VisualEffectObject>();
    }

    private void Start()
    {   
        float speed = 50f;
        bulletRigidbody.velocity = transform.forward * speed;

    }

    private void OnTriggerEnter(Collider other)
    {   
        // if(other.GetComponent<BulletTarget>() != null)
        // {
        //     //Hit Target
        // }
        // else
        // {
        //     //Hit something else
        // }
        Instantiate(vfxHit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
