using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;      
    public Vector3 offset = new Vector3(0, 5, -10);
    public float followSpeed = 5f;

    void LateUpdate()
    {
        Vector3 desiredPos = target.position + target.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}
