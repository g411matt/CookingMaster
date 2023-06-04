using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary class for the player, handles grabbing and placing of items
/// </summary>
public class Player : MonoBehaviour
{
    private Queue<GrabItem> _heldItems = new Queue<GrabItem>();
    private bool _positionLocked = false;
    private Interactable _availableInteraction = null;

    public bool CanTakeItem()
    {
        return _heldItems.Count < 2;
    }

    public bool HasItem()
    {
        return _heldItems.Count > 0;
    }
    public bool TryTakeItem(GrabItem item)
    {
        if (_heldItems.Count < 2)
        {
            _heldItems.Enqueue(item);
            return true;
        }
        return false;
    }

    public GrabItem PlaceItem()
    {
        if (_heldItems.Count > 0)
        {
            return _heldItems.Dequeue();
        }
        return null;
    }

    public void LockPlayer()
    {
        _positionLocked = true;
    }

    public void UnlockPlayer()
    {
        _positionLocked = false;
    }

    public bool CanMove()
    {
        return !_positionLocked;
    }

    public void TryInteract()
    {
        if (_availableInteraction != null)
        {
            _availableInteraction.Interact(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _availableInteraction = col.GetComponent<Interactable>();
    }

    void OnTriggerxit2D(Collider2D col)
    {
        _availableInteraction = null;
    }
}
