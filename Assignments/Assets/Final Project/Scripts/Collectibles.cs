using UnityEngine;

public class Collectibles : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Gamemanager.Instance.CollectItem();
        Destroy(transform.root.gameObject);
    }
}
