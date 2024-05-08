using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public CinemachineFreeLook cinemachineCamera;
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cinemachineCamera.m_XAxis.Value;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }
}
