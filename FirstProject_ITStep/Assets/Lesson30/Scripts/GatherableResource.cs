using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GatherableResource : MonoBehaviour
{
    [Header("Введи ID предмета (наприклад: wood, coal, stone)")]
    [SerializeField] private string _itemId = "wood";

    [Header("Налаштування")]
    [SerializeField] private int _amountPerGather = 1;
    [SerializeField] private int _health = 3; 

    public void Gather(InventorySystem inventory)
    {
        if (_health <= 0) return;

        if (!InventoryBootstrap.AllItems.TryGetValue(_itemId, out ItemDefinition itemDrop))
        {
            //Debug.LogError($"Предмет з ID '{_itemId}' не знайдено! Перевір правильність вводу.");
            return;
        }

        bool added = inventory.TryAddItem(itemDrop, _amountPerGather);

        if (added)
        {
            _health--;

            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}