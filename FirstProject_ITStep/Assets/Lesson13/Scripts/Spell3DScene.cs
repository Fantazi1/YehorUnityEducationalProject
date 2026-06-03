using System.Collections.Generic;
using UnityEngine;

public class Spell3DScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private GameObject _spikePrefab;
    [SerializeField] private GameObject _buildCubePrefab;

    [SerializeField] private Transform _spawnPoint;

    [Header("Fireball")]
    [SerializeField] private float _fireballForce = 20f;
    [SerializeField] private float _fireballMaxDistance = 20f;

    [Header("Hook")]
    [SerializeField] private float _hookDistance = 20f;
    [SerializeField] private float _hookMinDistance = 5f;
    [SerializeField] private float _hookPullSpeed = 10f;

    [Header("BuildCubes")]
    [SerializeField] private float _buildCubesDistance = 10f;
    [SerializeField] private float _buildCubesLifetime = 2f;

    [Header("Spikes")]
    [SerializeField] private float _spikeLifetime = 2f;

    [SerializeField] private int _maxSpikes = 5;

    private List<GameObject> _activeSpikes = new List<GameObject>();
    private List<GameObject> _activeBuildCubes = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CastFireball();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnSpike();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Hook();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            BuildCubesSpawn();
        }
    }

    private void CastFireball()
    {
        GameObject ball = Instantiate(_fireballPrefab, _spawnPoint.position, _spawnPoint.rotation);

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = _spawnPoint.forward * _fireballForce;
        }

        Destroy(ball, _fireballMaxDistance / _fireballForce);
    }

    private void SpawnSpike()
    {
        if (_activeSpikes.Count >= _maxSpikes)
        {
            Destroy(_activeSpikes[0]);
            _activeSpikes.RemoveAt(0);
        }

        Vector3 spawnPos = transform.position - ( new Vector3(0, 1, 0)) + transform.forward * 3f;

        GameObject spike = Instantiate(_spikePrefab, spawnPos, Quaternion.identity);

        _activeSpikes.Add(spike);

        Destroy(spike, _spikeLifetime);
        StartCoroutine(RemoveSpikeLater(spike));
    }

    private System.Collections.IEnumerator RemoveSpikeLater(GameObject spike)
    {
        yield return new WaitForSeconds(_spikeLifetime);

        if (_activeSpikes.Contains(spike))
        {
            _activeSpikes.Remove(spike);
        }
    }

    private void Hook()
    {
        RaycastHit hit;

        if (Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, out hit, _hookDistance))
        {
            if (hit.collider.CompareTag("HookableTag"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);

                if (distance >= _hookMinDistance)
                {
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        Vector3 direction = (transform.position - hit.transform.position).normalized;

                        rb.linearVelocity = direction * _hookPullSpeed;
                    }
                }
            }
        }
    }

    private void BuildCubesSpawn()
    {
        RaycastHit hit;

        Camera mainCamera = Camera.main;

        if (mainCamera == null) return;

        Vector3 rayStart = mainCamera.transform.position;
        Vector3 rayDirection = mainCamera.transform.forward;

        if (Physics.Raycast(rayStart, rayDirection, out hit, _buildCubesDistance))
        {
            Vector3 spawnPosition = hit.point + (hit.normal * 0.5f);
            GameObject buildCubes = Instantiate(_buildCubePrefab, spawnPosition, Quaternion.identity);

            _activeBuildCubes.Add(buildCubes);

            Destroy(buildCubes, _buildCubesLifetime);
            StartCoroutine(RemoveSpikeLater(buildCubes));
        }
    }
}

