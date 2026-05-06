using UnityEngine;
using static UnityEditor.Progress;

public class ItemCollecting : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemPrefabUser;
    [SerializeField] private GameObject[] _itemPrefabObj;

    private void OnTriggerEnter(Collider other)
    {
        for (int i=0; i< _itemPrefabUser.Length; i++) {
            if (other.CompareTag(_itemPrefabUser[i].tag))
            {
                _itemPrefabUser[i].SetActive(true);
                _itemPrefabObj[i].SetActive(false);

                break;
            }
        }
    }
}