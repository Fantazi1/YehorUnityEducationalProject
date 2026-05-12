using UnityEngine;
using static UnityEditor.Progress;

public class ArmorCollecting : MonoBehaviour
{
    [SerializeField] private GameObject _armorPrefabUserActive;
    [SerializeField] private GameObject _armorPrefabObj;

    private bool isAnyActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_armorPrefabUserActive.tag))
        {
            _armorPrefabUserActive.SetActive(true);
            _armorPrefabObj.SetActive(false);
        }
    }
}