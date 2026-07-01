using UnityEngine;

public class Building3DMiniRTS : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _spawnCount = 3;

    private int _storedResources;

    public int StoredResources => _storedResources;

    private void Awake()
    {
        if (_spawnPoint == null)
            _spawnPoint = transform;
    }

    public void SpawnUnits()
    {
        if (_unitPrefab == null)
            return;

        for (var i = 0; i < _spawnCount; i++)
        {
            var offset = Random.insideUnitSphere * 2f;
            offset.y = 0;

            Instantiate(_unitPrefab, _spawnPoint.position + new Vector3(0, 0.5f, 0) + offset, Quaternion.identity);
        }
    }

    public void Deposit(int amount)
    {
        _storedResources += amount;
    }
}