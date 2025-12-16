using UnityEngine;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public List<Transform> spawnPoints;
    public int collectiblesToSpawn = 5;

    void Start()
    {
        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        List<Transform> shuffled = new List<Transform>(spawnPoints);

        for (int i = 0; i < shuffled.Count; i++)
        {
            int rand = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }

        for (int i = 0; i < collectiblesToSpawn; i++)
        {
            Instantiate(
                collectiblePrefab,
                shuffled[i].position,
                Quaternion.identity
            );
        }
    }
}
