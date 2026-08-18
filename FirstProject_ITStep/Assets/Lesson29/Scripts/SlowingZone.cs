using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SlowingZone : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private Vector3 zoneSize = new Vector3(5f, 2f, 5f);
    [SerializeField] private LayerMask playerLayer;

    [Header("Damage")]
    [SerializeField] private float slowliness = 0.5f;
    [SerializeField] private float slowingInterval = 1f;

    [Header("Danger Escalation")]
    [SerializeField] private bool increaseSlowlinessOverTime = true;
    [SerializeField] private float slowingIncreasePerSecond = 0.2f;
    [SerializeField] private float maxSlowing = 10f;

    private float slowingTimer;
    private float timeInsideZone;

    private PlayerHealth playerHealth;

    [SerializeField] private FirstPersonController _firstPersonController;
    private float base_walk_speed;
    private float base_run_speed;

    private void Start()
    {
        //_firstPersonController = playerHealth.gameObject.GetComponent<FirstPersonController>();
        base_walk_speed = _firstPersonController.M_WalkSpeed;
        base_run_speed = _firstPersonController.M_RunSpeed;
    }

    private void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        Collider[] objectsInsideZone = Physics.OverlapBox(
            transform.position,
            zoneSize / 2f,
            transform.rotation,
            playerLayer
        );

        bool playerFound = false;

        foreach (Collider objectCollider in objectsInsideZone)
        {
            PlayerHealth health = objectCollider.GetComponentInParent<PlayerHealth>();

            if (health == null)
                continue;

            playerFound = true;

            if (playerHealth != health)
            {
                playerHealth = health;
                slowingTimer = 0f;
                timeInsideZone = 0f;
            }

            break;
        }

        if (playerFound)
        {
            HandlePlayerInside();
        }
        else
        {
            HandlePlayerOutside();
        }
    }

    private void HandlePlayerInside()
    {
        timeInsideZone += Time.deltaTime;
        slowingTimer += Time.deltaTime;

        if (slowingTimer >= slowingInterval)
        {
            float currentSlowing = slowliness;

            if (increaseSlowlinessOverTime)
            {
                currentSlowing += slowingIncreasePerSecond * timeInsideZone;

                currentSlowing = Mathf.Clamp(
                    currentSlowing,
                    0f,
                    maxSlowing
                );
            }

            if (_firstPersonController.M_WalkSpeed > currentSlowing)
            {
                _firstPersonController.M_WalkSpeed -= currentSlowing;
            }
            if (_firstPersonController.M_RunSpeed > currentSlowing)
            {
                _firstPersonController.M_RunSpeed -= currentSlowing;
            }

            //playerHealth.TakeDamage(currentSlowing);

            slowingTimer = 0f;
        }
    }

    private void HandlePlayerOutside()
    {
        slowingTimer = 0f;
        timeInsideZone = 0f;
        playerHealth = null;
        _firstPersonController.M_WalkSpeed = base_walk_speed;
        _firstPersonController.M_RunSpeed = base_run_speed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position,
            transform.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(
            Vector3.zero,
            zoneSize
        );
    }
}