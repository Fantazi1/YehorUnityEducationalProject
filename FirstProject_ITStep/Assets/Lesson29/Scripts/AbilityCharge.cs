using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityCharge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _firePoint; 
    [SerializeField] private GameObject _projectilePrefab;

    [Header("Ability Charge Settings")]
    [SerializeField] private float _baseSpeed = 10f; 
    [SerializeField] private float _maxBonusSpeed = 30f; 
    [SerializeField] private float _chargeRate = 20f; 
    [SerializeField] private float _projectileLifetime = 5f; 

    private float _currentCharge = 0f;
    private float _maxCharge = 100f;

    private void Update()
    {
        if (Mouse.current == null) return;

        // Заряджання при утриманні ПКМ
        if (Mouse.current.rightButton.isPressed)
        {
            _currentCharge += _chargeRate * Time.deltaTime;
            _currentCharge = Mathf.Clamp(_currentCharge, 0f, _maxCharge);
        }
        // Постріл при відпусканні кнопки
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            FireChargedAbility();
        }
    }

    private void FireChargedAbility()
    {
        if (_projectilePrefab == null || _currentCharge <= 0f) return;

        Transform spawnPoint = _firePoint != null ? _firePoint : transform;

        float chargePercent = _currentCharge / _maxCharge;
        float finalSpeed = _baseSpeed + (_maxBonusSpeed * chargePercent);

        GameObject projectile = Instantiate(_projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        projectile.transform.localScale *= (1f + chargePercent);

        if (projectile.TryGetComponent<Rigidbody>(out var projRb)) 
        {
            projRb.linearVelocity = spawnPoint.forward * finalSpeed;
        }

        Destroy(projectile, _projectileLifetime);

        _currentCharge = 0f;
    }
}