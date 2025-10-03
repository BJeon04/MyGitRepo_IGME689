using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float timeBonus = 5f;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CollectCollectable(timeBonus);
            Destroy(gameObject);
        }
    }
}
