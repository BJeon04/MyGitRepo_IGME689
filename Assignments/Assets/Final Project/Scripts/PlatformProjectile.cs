using UnityEngine;

public class PlatformProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float stopDistance = 5f;

    private bool isMoving = true;

    void Update()
    {
        if (!isMoving) return;

        Vector3 moveDir = transform.forward;

        if (Physics.Raycast(transform.position, moveDir, out RaycastHit hit, stopDistance))
        {
            StopAndPlace();
            return;
        }

        transform.position += moveDir * speed * Time.deltaTime;
    }

    void StopAndPlace()
    {
        isMoving = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}
