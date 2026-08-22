using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Налаштування")]
    [SerializeField] private float _interactRange = 4f;

    private Camera _playerCamera;
    private InventorySystem _inventorySystem;
    private FirstPersonController _playerController;

    private void Start()
    {
        _playerCamera = Camera.main;
        _inventorySystem = FindFirstObjectByType<InventorySystem>();

        _playerController = GetComponentInParent<FirstPersonController>();
    }

    private void Update()
    {
        if (_playerController != null && _playerController.isUIOpen) return;

        if (Input.GetMouseButtonDown(0))
        {
            GatherResource();
        }
    }

    private void GatherResource()
    {
        if (_playerCamera == null || _inventorySystem == null) return;

        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange))
        {
            GatherableResource resource = hit.collider.GetComponent<GatherableResource>();

            if (resource != null)
            {
                resource.Gather(_inventorySystem);
            }
        }
    }
}