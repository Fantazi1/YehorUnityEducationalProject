using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Transform[] waypoints;

    [SerializeField] private float spawnRate = 2f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnEnemy();

            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemyObject = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity);

        EnemyMovement enemyMovement =
            enemyObject.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            enemyMovement.Initialize(waypoints);
        }
    }
}