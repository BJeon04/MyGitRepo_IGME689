using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 9f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpForce = 4f;

    public CharacterController controller;
    private Transform cam;

    private float xRotation = 0f;
    private Vector3 velocity;

    private Vector3 externalMomentum;

    public bool controlsEnabled = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!controlsEnabled)
            return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float finalSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        controller.Move(move * finalSpeed * Time.deltaTime);

        // Gravity
     
        if (externalMomentum.magnitude > 0.1f)
        {
            controller.Move(externalMomentum * Time.deltaTime);

            externalMomentum = Vector3.Lerp(
                externalMomentum,
                Vector3.zero,
                4f * Time.deltaTime
            );
        }


        // Gravity
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            velocity.y = jumpForce;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ResetVerticalVelocity()
    {
        velocity.y = 0f;
    }

    public void AddBoost(float boost)
    {
        velocity.y = boost;
       
    }

    public void AddMomentum(Vector3 momentum)
    {
        externalMomentum = momentum;
    }

    public void EnableControls(bool enable)
    {
        controlsEnabled = enable;

        if (!enable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
