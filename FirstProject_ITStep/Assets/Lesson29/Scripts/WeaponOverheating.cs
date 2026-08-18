using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponOverheating : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _firePoint;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _baseSpeed = 20f;
    [SerializeField] private float _projectileLifetime = 5f;
    [SerializeField] private float _fireRate = 0.15f;

    [Header("Heat Settings")]
    [SerializeField] private float _maxHeat = 100f;
    [SerializeField] private float _heatPerShot = 15f;
    [SerializeField] private float _coolingRate = 25f;

    private float _currentHeat = 0f;
    private float _nextFireTime = 0f;
    private bool _isOverheated = false;

    private void Update()
    {
        HandleCooling();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (_isOverheated) return;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            if (Time.time >= _nextFireTime)
            {
                Shoot();
            }
        }
    }

    private void HandleCooling()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.isPressed || _isOverheated)
        {
            if (_currentHeat > 0)
            {
                _currentHeat -= _coolingRate * Time.deltaTime;
                _currentHeat = Mathf.Clamp(_currentHeat, 0f, _maxHeat);
            }

            if (_isOverheated && _currentHeat <= 0.1f)
            {
                _isOverheated = false;
                _currentHeat = 0f;
            }
        }
    }

    private void Shoot()
    {
        _nextFireTime = Time.time + _fireRate;
        _currentHeat += _heatPerShot;

        if (_projectilePrefab != null)
        {
            Transform spawnPoint = _firePoint != null ? _firePoint : transform; 
            GameObject projectile = Instantiate(_projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            if (projectile.TryGetComponent<Rigidbody>(out var projRb)) 
            {
                projRb.linearVelocity = spawnPoint.forward * _baseSpeed;
            }

            Destroy(projectile, _projectileLifetime); 
        }

        if (_currentHeat >= _maxHeat)
        {
            _isOverheated = true;
        }
    }
}