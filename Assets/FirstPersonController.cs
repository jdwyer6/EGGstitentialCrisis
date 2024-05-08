using UnityEngine;
using Cinemachine;
using System.Collections;

public class FirstPersonController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float lookSpeed = 2f;
    private CinemachineVirtualCamera cinemachineCamera;
    private float cameraVerticalAngle;
    public float jumpForce = 5f;
    public LayerMask groundLayer; // Assign a layer for the ground in the inspector
    public Transform groundCheck; // Assign or create a child GameObject at the bottom of the player for ground checking
    public float groundDistance = 0.4f;
    private Rigidbody rb;
    public float additionalGravity = 30f;
    

    private void Start()
    {
        cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Look();

        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (!isGrounded)
        {
            ApplyAdditionalGravity();
        }

    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
    }

    void Look()
    {
        float lookX = Input.GetAxisRaw("Mouse X") * lookSpeed;
        float lookY = Input.GetAxisRaw("Mouse Y") * lookSpeed;

        transform.Rotate(new Vector3(0f, lookX, 0f), Space.Self);

        cameraVerticalAngle -= lookY;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        cinemachineCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ApplyAdditionalGravity()
    {
        rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }
    
}
