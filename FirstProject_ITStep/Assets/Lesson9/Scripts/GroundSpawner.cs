using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private Transform player;

    private float spawnZ = 0f;
    private float roadLength = 10f;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnerRoad();
        }
    }

    void Update()
    {
        if (player.position.z + 50 > spawnZ)
        {
            SpawnerRoad();
        }
    }

    void SpawnerRoad()
    {
        Instantiate(roadPrefab, new Vector3(0, 0, spawnZ), Quaternion.identity);

        spawnZ += roadLength;
    }
}