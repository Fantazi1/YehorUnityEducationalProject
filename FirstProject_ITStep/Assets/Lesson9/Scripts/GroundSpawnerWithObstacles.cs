using System.Collections.Generic;
using UnityEngine;

public class GroundSpawnerWithObstacles : MonoBehaviour
{
    [SerializeField] private GameObject[] roadPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 roadWidthCoordinateX = new Vector2(-3f, 3f);
    [SerializeField] private GameObject coins;
    [SerializeField] private GameObject crystals;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject woodenPlank;
    private Stack<GameObject> spawnedRoadsStack = new Stack<GameObject>();
    private Stack<GameObject> spawnedCoinsOrCrystalsStack = new Stack<GameObject>();
    private float startRoadsNum = 10f;
    private float spawnZ = 0f;
    private float roadLength = 10f;
    private float roadNumToSpawnCrystal = 3f;
    private float roadNumToSpawnBarrel = 2f;
    private float roadNumBarrel = 0f;
    private float roadNum = 0f;
    
    private float maxOldRoads = 15f;
    private float maxOldCoinsOrCrystals = 10f;
    void Start()
    {
        for (int i = 0; i < startRoadsNum; i++)
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
        int randomRoadIndex = Random.Range(0, roadPrefab.Length);
        GameObject randRoad = roadPrefab[randomRoadIndex];
        GameObject road = Instantiate(randRoad, new Vector3(0, 0, spawnZ), Quaternion.identity);

        AddToStackAndCheck(spawnedRoadsStack, road, maxOldRoads);

        SpawnerCoinsAndCrystals(coins);
        roadNum++;
        roadNumBarrel++;
        if (roadNum >= roadNumToSpawnCrystal)
        {
            roadNum = 0;
            SpawnerCoinsAndCrystals(crystals);
        }
        if (roadNumBarrel >= roadNumToSpawnBarrel)
        {
            roadNumBarrel = 0;
            GameObject chosenPrefab = (Random.value < 0.5f) ? barrel : woodenPlank; //Picking random barrel or wooden plank obstacle
            SpawnerCoinsAndCrystals(chosenPrefab);
        }
        
        spawnZ += roadLength;
    }

    private void SpawnerCoinsAndCrystals(GameObject coinsOrCrystals)
    {
        float randomX = Random.Range(roadWidthCoordinateX[0], roadWidthCoordinateX[1]);
        Vector3 coinSpawnPointPos = new Vector3(randomX, 2, spawnZ);

        Collider[] colliders = Physics.OverlapSphere(coinSpawnPointPos, 1f);

        if (colliders.Length == 0)
        {
            GameObject coinsOrCrystalsSpawned = Instantiate(coinsOrCrystals, new Vector3(randomX, 1, spawnZ), Quaternion.identity);
            AddToStackAndCheck(spawnedCoinsOrCrystalsStack, coinsOrCrystalsSpawned, maxOldCoinsOrCrystals);
        }
    }

    private void AddToStackAndCheck(Stack<GameObject> stackObjects, GameObject spawnedObject, float maxLimit)
    {
        stackObjects.Push(spawnedObject);
        if (stackObjects.Count > maxLimit)
        {
            RemoveOldestFromStack(stackObjects);
        }
    }

    private void RemoveOldestFromStack(Stack<GameObject> spawnedObjects)
    {
        Stack<GameObject> tempStack = new Stack<GameObject>();

        while (spawnedObjects.Count > 1)
        {
            tempStack.Push(spawnedObjects.Pop());
        }

        GameObject oldestObject = spawnedObjects.Pop();
        if (oldestObject != null)
        {
            Destroy(oldestObject);
        }

        while (tempStack.Count > 0)
        {
            spawnedObjects.Push(tempStack.Pop());
        }
    }
}