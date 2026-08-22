using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler
{
    private InventoryUI _inventoryUI;
    private InventorySlotArea _area;
    private int _index;

    public InventorySlotArea Area => _area;
    public int Index => _index;

    public void Initialize(InventoryUI inventoryUI, InventorySlotArea area, int index)
    {
        _inventoryUI = inventoryUI;
        _area = area;
        _index = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool exactSplit =
            eventData.button == PointerEventData.InputButton.Right &&
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));

        _inventoryUI.HandleSlotClick(this, eventData.button, exactSplit);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        _inventoryUI.BeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        _inventoryUI.Drag(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        _inventoryUI.EndDrag(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        InventorySlotUI source = eventData.pointerDrag != null
            ? eventData.pointerDrag.GetComponent<InventorySlotUI>()
            : null;

        if (source != null)
        {
            _inventoryUI.Drop(source, this);
        }
    }
}