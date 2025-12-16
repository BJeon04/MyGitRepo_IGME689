using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    public int floorNumber;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (floorNumber == 2)
            {
            }
        }
    }
}
