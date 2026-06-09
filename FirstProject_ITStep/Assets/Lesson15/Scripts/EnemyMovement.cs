using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private Transform[] waypoints;

    private int currentWaypointIndex;

    public void Initialize(Transform[] pathWaypoints)
    {
        waypoints = pathWaypoints;

        currentWaypointIndex = 0;
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        if (currentWaypointIndex >= waypoints.Length)
        {
            Destroy(gameObject);

            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetWaypoint.position,
            speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) <= 0.1f)
        {
            currentWaypointIndex++;
        }
    }
}