using UnityEngine;


public class DroneController : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 100f;
    public float ascendSpeed = 50f;
    public float rotationSpeed = 100f;

    [Header("Camera Follow")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 15f, -30f);
    public float cameraFollowSmoothness = 5f;

    private void Update()
    {
        if (MidTermGameManager.GameOver)
            return;

        HandleMovement();
        HandleRotation();
        HandleCameraFollow();
        HandleScanInput();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0f;
        if (Input.GetKey(KeyCode.Q)) moveY = 1f;
        if (Input.GetKey(KeyCode.E)) moveY = -1f;

        Vector3 move = new Vector3(moveX, moveY * (ascendSpeed / moveSpeed), moveZ);
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleRotation()
    {
        float yaw = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);
    }

    private void HandleCameraFollow()
    {
        if (cameraTransform == null) return;

        Vector3 targetPos = transform.position + transform.TransformDirection(cameraOffset);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * cameraFollowSmoothness);
        cameraTransform.LookAt(transform.position + Vector3.up * 5f);
    }

    private void HandleScanInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 dronePos = transform.position;

            foreach (var zone in HeatZone.AllZones)
            {
                if (zone != null && zone.ContainsPoint(dronePos))
                {
                    zone.CoolDownStep(Time.deltaTime); 
                }
            }
        }
    }

    public void IncreaseStats()
    {
        moveSpeed += 50f;
        ascendSpeed += 20f;

        Debug.Log($"Drone leveled up! New moveSpeed: {moveSpeed}, ascendSpeed: {ascendSpeed}");
    }
}
