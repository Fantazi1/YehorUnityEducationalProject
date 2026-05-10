using UnityEngine;
using static UnityEditor.Progress;

public class ItemCollecting : MonoBehaviour
{
    [SerializeField] private GameObject[] _itemPrefabUserActive;
    [SerializeField] private GameObject[] _itemPrefabUserNotChoosen;
    [SerializeField] private GameObject[] _itemPrefabObj;

    private bool isAnyActive = false;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _itemPrefabUserActive.Length; i++) {

            for (int j = 0; j < _itemPrefabUserActive.Length; j++)
            {
                if (_itemPrefabUserActive[j].activeInHierarchy && _itemPrefabUserActive[j].tag != "QuiverTag" && _itemPrefabUserActive[j].tag != "ShieldTag")
                {
                    isAnyActive = true;
                }
            }

            if (other.CompareTag(_itemPrefabUserActive[i].tag))
            {
                
                
                if ((isAnyActive && _itemPrefabUserActive[i].tag == "ShieldTag") || (isAnyActive && _itemPrefabUserActive[i].tag == "QuiverTag"))
                {
                    _itemPrefabUserActive[i].SetActive(true);
                    _itemPrefabObj[i].SetActive(false);
                }
                else if (isAnyActive)
                {
                    _itemPrefabUserNotChoosen[i].SetActive(true);
                    _itemPrefabUserActive[i].SetActive(false);
                    _itemPrefabObj[i].SetActive(false);
                    isAnyActive = false;
                }
                else {
                    _itemPrefabUserActive[i].SetActive(true);
                    _itemPrefabObj[i].SetActive(false);
                }

                break;
            }
        }
    }
}