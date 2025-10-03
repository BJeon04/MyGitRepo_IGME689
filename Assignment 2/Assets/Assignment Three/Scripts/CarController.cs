using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float acceleration = 500f;
    public float steering = 2f;
    public float maxSpeed = 20f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");  
        float steerInput = Input.GetAxis("Horizontal"); 

        //  Apply forward movement
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * acceleration, ForceMode.Acceleration);
        }

        // Apply steering (only if moving)
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * steerInput * steering));
        }


    }
}