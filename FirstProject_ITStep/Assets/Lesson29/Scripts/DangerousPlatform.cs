using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DangerousPlatform : MonoBehaviour
{
    private enum PlatformState { Idle, Counting, Blinking, Hidden, Restoring }

    [Header("Zone Settings")]
    [SerializeField] private Vector3 _zoneSize = new Vector3(2f, 1f, 2f);
    [SerializeField] private Vector3 _zoneOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private LayerMask _playerLayer;

    [Header("Timings")]
    [SerializeField] private float _timeToBlink = 3f;   
    [SerializeField] private float _blinkDuration = 2f; 
    [SerializeField] private float _restoreDelay = 2f;  

    [Header("Blink Settings")]
    [SerializeField] private float _blinkInterval = 0.15f;
    [SerializeField] private string _playerTag = "Player";

    private PlatformState _currentState = PlatformState.Idle;
    private float _timer = 0f;
    private float _restoreTimer = 0f;
    private float _blinkTimer = 0f;

    private MeshRenderer _meshRenderer;
    private Collider _platformCollider;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _platformCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        bool isPlayerOnPlatform = CheckForPlayer();

        switch (_currentState)
        {
            case PlatformState.Idle:
                if (isPlayerOnPlatform)
                {
                    _currentState = PlatformState.Counting;
                    _timer = 0f;
                }
                break;

            case PlatformState.Counting:
                if (!isPlayerOnPlatform)
                {
                    ResetToIdle();
                    break;
                }

                _timer += Time.deltaTime;
                if (_timer >= _timeToBlink)
                {
                    _currentState = PlatformState.Blinking;
                    _timer = 0f;
                }
                break;

            case PlatformState.Blinking:
                if (!isPlayerOnPlatform)
                {
                    ResetToIdle();
                    break;
                }

                _timer += Time.deltaTime;
                HandleBlinking();

                if (_timer >= _blinkDuration)
                {
                    HidePlatform();
                }
                break;

            case PlatformState.Hidden:
                if (!isPlayerOnPlatform)
                {
                    _currentState = PlatformState.Restoring;
                    _restoreTimer = 0f;
                }
                break;

            case PlatformState.Restoring:
                if (isPlayerOnPlatform)
                {
                    _currentState = PlatformState.Hidden;
                    _restoreTimer = 0f;
                }
                else
                {
                    _restoreTimer += Time.deltaTime;
                    if (_restoreTimer >= _restoreDelay)
                    {
                        RestorePlatform();
                    }
                }
                break;
        }
    }

    private bool CheckForPlayer()
    {
        Vector3 center = transform.position + _zoneOffset;
        Collider[] hits = Physics.OverlapBox(center, _zoneSize / 2f, transform.rotation, _playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit == _platformCollider || hit.gameObject == gameObject)
                continue;

            bool isPlayerByTag = hit.CompareTag(_playerTag);
            bool isPlayerByComponent = hit.GetComponentInParent<FirstPersonController>() != null;

            if (isPlayerByTag || isPlayerByComponent)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleBlinking()
    {
        _blinkTimer += Time.deltaTime;
        if (_blinkTimer >= _blinkInterval)
        {
            _blinkTimer = 0f;
            _meshRenderer.enabled = !_meshRenderer.enabled;
        }
    }

    private void HidePlatform()
    {
        _currentState = PlatformState.Hidden;
        _meshRenderer.enabled = false;
        _platformCollider.enabled = false; 
    }

    private void RestorePlatform()
    {
        _currentState = PlatformState.Idle;
        _timer = 0f;
        _restoreTimer = 0f;
        _meshRenderer.enabled = true;
        _platformCollider.enabled = true;
    }

    private void ResetToIdle()
    {
        _currentState = PlatformState.Idle;
        _timer = 0f;
        _meshRenderer.enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.matrix = Matrix4x4.TRS(transform.position + _zoneOffset, transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, _zoneSize);
    }
}