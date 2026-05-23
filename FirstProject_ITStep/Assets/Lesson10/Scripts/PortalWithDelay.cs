using System.Collections;
using UnityEngine;

public class PortalWithDelay : MonoBehaviour
{
    [SerializeField] private Transform _exitPoint;
    [Tooltip("Time Delay For The Portal")]
    [SerializeField] private float cooldownTime = 1.0f;
    private static float nextTriggerTime = 0.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time >= nextTriggerTime)
        {
            Teleport(collision);
            nextTriggerTime = Time.time + cooldownTime;
        }
    }

    private void Teleport(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = _exitPoint.position;
        }
    }
}
