using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class LookAtObjectAnimationRigging : MonoBehaviour
{
    public Rig rig;
    private float targetWeight;
    public Transform target;
    private void Awake()
    {
        rig = GetComponent<Rig>();
    }

    private void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * 10f);
    }
    public void SetTargetWeight(float weight)
    {
        targetWeight = weight;
    }
}
