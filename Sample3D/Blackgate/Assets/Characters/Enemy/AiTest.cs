using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiTest : MonoBehaviour
{
    public ThirdPersonShooterController tps;
    public float fov = 120f;
    public float viewDistance = 10f;
    private bool isAware = false;
    private NavMeshAgent agent;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(isAware)
        {
            /*chase player*/
            agent.SetDestination(tps.transform.position);
        }
        else
        {
            SearchForPlayer();
        }
    }

    public void SearchForPlayer()
    {
        if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(tps.transform.position)) < fov / 2f)
        {
            if(Vector3.Distance(tps.transform.position, transform.position) < viewDistance)
            {
                OnAware();
            }
        }
    }

    public void OnAware()
    {
        isAware = true;
    }
}
