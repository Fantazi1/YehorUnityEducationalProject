using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;

    [Header("Regeneration")]
    [SerializeField] private float timeToStartRegeneration = 5f;
    [SerializeField] private float regenerationPerSecond = 10f;

    [Header("Movement Detection")]
    [SerializeField] private float movementThreshold = 0.01f;

    [Header("Respawn Protection")]
    [SerializeField] private float invulnerabilityAfterRespawn = 2f;

    private float stationaryTimer;
    private float invulnerabilityTimer;

    private Vector3 lastPosition;

    private void Start()
    {
        currentHealth = maxHealth;

        lastPosition = transform.position;
    }

    private void Update()
    {
        CheckMovement();
        HandleRegeneration();
        HandleInvulnerability();
    }

    private void CheckMovement()
    {
        Vector3 currentPosition = transform.position;
        
        Vector3 currentXZ = new Vector3(
            currentPosition.x,
            0f,
            currentPosition.z
        );

        Vector3 lastXZ = new Vector3(
            lastPosition.x,
            0f,
            lastPosition.z
        );

        float distance = Vector3.Distance(currentXZ, lastXZ);

        if (distance > movementThreshold)
        {
            stationaryTimer = 0f;
        }
        else
        {
            stationaryTimer += Time.deltaTime;
        }

        lastPosition = currentPosition;
    }

    private void HandleRegeneration()
    {
        if (currentHealth >= maxHealth)
            return;

        if (stationaryTimer < timeToStartRegeneration)
            return;

        currentHealth += regenerationPerSecond * Time.deltaTime;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );
    }

    private void HandleInvulnerability()
    {
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (invulnerabilityTimer > 0f)
            return;

        if (currentHealth <= 0f)
            return;

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        Respawn();
    }

    private void Respawn()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn Point is not assigned!");
            return;
        }
        
        CharacterController characterController =
            GetComponent<CharacterController>();

        if (characterController != null)
        {
            characterController.enabled = false;
        }
        
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        
        if (characterController != null)
        {
            characterController.enabled = true;
        }
        
        currentHealth = maxHealth;
        
        stationaryTimer = 0f;
        
        lastPosition = transform.position;
        
        invulnerabilityTimer = invulnerabilityAfterRespawn;

        Debug.Log("Player respawned at: " + respawnPoint.position);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetStationaryTimer()
    {
        return stationaryTimer;
    }

    public float GetInvulnerabilityTimer()
    {
        return invulnerabilityTimer;
    }
}