using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

using UnityEngine.Animations.Rigging;


public class ThirdPersonShooterController : MonoBehaviour
{

    
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Gun Gun;
    [SerializeField] private Transform spawnBulletPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    // [SerializeField] private LookAtObjectAnimationRigging headRig; /*get the head riggin componenet*/
    // [SerializeField] private LookAtObjectAnimationRigging bodyRig; /*get the head riggin componenet*/
    [SerializeField] private Rig aimRig;
    private float aimRigWeight;

    public void Awake()
    {   
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    /*add canvas to crossair only when shooting*/
    void Update()
    {   
        Vector3 mouseWorldPosition = Vector3.zero;

        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 20f);

        /*getting the screen center so it will work both with controller(if mouse is not connected)*/
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if(starterAssetsInputs.aim)
        {
            OnAimStarted();

            /*change to aiming layer*/
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1),1f,Time.deltaTime * 10f));

            /*this is basically to make the character look where we aim*/
            Vector3 wolrdAimTarget = wolrdAimTarget = mouseWorldPosition;
            wolrdAimTarget.y = transform.position.y;
            Vector3 aimDirection = (wolrdAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            // headRig.SetTargetWeight(1f); /*head look*/
            // bodyRig.SetTargetWeight(1f); /*body look*/
            
        }
        else
        {
            OnAimStopped();

            /*change to normal walking layer*/
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1),0f,Time.deltaTime * 10f));

            // headRig.SetTargetWeight(0f);/*head lookreset*/
            // bodyRig.SetTargetWeight(0f);/*body lookreset*/
        }

        if(starterAssetsInputs.shoot)
        {   
            //shot without bullets
            // if(hitTransform != null)
            // {
            //     //hit something
            //     Instantiate(vfxHit, transform.position, Quaternion.identity);
            // }
            //shoot with bullets
            
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Gun.Shoot(aimDir); /*invokes the gun type shooting*/
            starterAssetsInputs.shoot = false;
        } 
    }
    private void OnAimStopped()
    {
        aimVirtualCamera.gameObject.SetActive(false);
        thirdPersonController.SetSensitivity(normalSensitivity);
        thirdPersonController.SetRotateOnMove(true);
        aimRigWeight = 0f;
    }

    private void OnAimStarted()
    {   
        aimVirtualCamera.gameObject.SetActive(true);
        thirdPersonController.SetSensitivity(aimSensitivity);
        thirdPersonController.SetRotateOnMove(false);
        aimRigWeight = 1f;
    }
}
