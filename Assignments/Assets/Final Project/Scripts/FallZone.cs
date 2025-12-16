using UnityEngine;

public class FallZone : MonoBehaviour
{
    public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CharacterController cc = other.GetComponent<CharacterController>();
        cc.enabled = false;
        other.transform.position = respawnPoint.position;
        cc.enabled = true;
    }
}
