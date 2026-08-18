using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileIncreasedSpeed : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _firePoint;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _baseSpeed = 10f;
    [SerializeField] private float _maxBonusSpeed = 20f;
    [SerializeField] private float _chargeRate = 5f;
    [SerializeField] private float _projectileLifetime = 5f; // Час життя снаряду в секундах

    private float _standTimer;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Перевіряємо, чи гравець стоїть на місці
        if (_rb != null && _rb.linearVelocity.sqrMagnitude < 0.01f)
        {
            _standTimer += Time.deltaTime;
        }
        else
        {
            _standTimer = 0f;
        }

        // Постріл на ЛІВУ кнопку миші
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_projectilePrefab == null) return;

        Transform spawnPoint = _firePoint != null ? _firePoint : transform;

        float bonusSpeed = Mathf.Min(_standTimer * _chargeRate, _maxBonusSpeed);
        float finalSpeed = _baseSpeed + bonusSpeed;

        GameObject projectile = Instantiate(_projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        if (projectile.TryGetComponent<Rigidbody>(out var projRb))
        {
            projRb.linearVelocity = spawnPoint.forward * finalSpeed;
        }

        // Автоматичне видалення снаряду з сцени через _projectileLifetime секунд
        Destroy(projectile, _projectileLifetime);

        _standTimer = 0f;
    }
}
