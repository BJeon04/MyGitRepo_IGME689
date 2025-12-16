using UnityEngine;

public class FinalCollectable : MonoBehaviour
{
    public System.Action OnCollected;

    public void Interact()
    {
        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}
