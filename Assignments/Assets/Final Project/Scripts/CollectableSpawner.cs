using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    public GameObject collectablePrefab;

    void Start()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0); 

        GameObject obj = Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);

        var c = obj.GetComponent<FinalCollectable>();
        c.OnCollected += OnCollectedItem;
    }

    void OnCollectedItem()
    {
        Debug.Log("Item collected! Unlocking stairs...");

    }
}
