using UnityEngine;
using UnityEngine.Events;

public class LeverInteractable : InteractableBase
{
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private bool _oneShot;

    private bool _pressed;

    public override void Interact()
    {
        if (!CanInteract)
        {
            return;
        }

        if (_oneShot && _pressed)
        {
            return;
        }

        _pressed = true;
        _onPressed?.Invoke();
    }
}