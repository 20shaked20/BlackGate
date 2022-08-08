using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class SwimmingController : MonoBehaviour
{

    private ThirdPersonController thirdPersonController;
    Animator _animator;

    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (thirdPersonController.isSwimming)
            // SwimmingGravity();

    }

    /*simple test, will create swimming script in a bit */
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            _animator.SetLayerWeight(2, Mathf.Lerp(_animator.GetLayerWeight(2), 1f, Time.deltaTime * 100f));
            _animator.SetLayerWeight(0, Mathf.Lerp(_animator.GetLayerWeight(0), 1f, Time.deltaTime * 100f));
            thirdPersonController.isSwimming = true;
            thirdPersonController.MoveSpeed = Mathf.Lerp(3.5f,1f,Time.deltaTime *10f);
            thirdPersonController.SprintSpeed = Mathf.Lerp(5f,1f,Time.deltaTime *10f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetLayerWeight(0, Mathf.Lerp(_animator.GetLayerWeight(0), 1f, Time.deltaTime * 10f));
        _animator.SetLayerWeight(2, 0);
        thirdPersonController.isSwimming = false;
        thirdPersonController.MoveSpeed = 2.0f;
        thirdPersonController.SprintSpeed = 5.335f;
    }

    private void SwimmingGravity()
    {
        /*this method controls the up and down gravity checks*/
        CharacterController controller = thirdPersonController.GetComponent<CharacterController>();
        Vector3 up = Vector3.zero;
        // controller.Move(up);
        if(Input.GetKeyDown(KeyCode.Space))
        {   
            up.y = 10f;
            controller.Move(up);
        }
    }
}
