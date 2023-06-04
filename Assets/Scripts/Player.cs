using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Primary class for the player, handles grabbing and placing of items
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// First held item slot, the item that drops first
    /// </summary>
    [SerializeField]
    private Transform _slot1 = null;
    /// <summary>
    /// Second held item slot, items move to slot 1 after slot 1 empties
    /// </summary>
    [SerializeField]
    private Transform _slot2 = null;
    /// <summary>
    /// The player's currently held items, they can only hold 2 and always drop them in acquistion order
    /// </summary>
    private Queue<GrabItem> _heldItems = new Queue<GrabItem>();
    /// <summary>
    /// Whether or not the player can move
    /// </summary>
    private bool _positionLocked = false;
    /// <summary>
    /// The interactable the player is currently in range of
    /// </summary>
    private Interactable _availableInteraction = null;

    /// <summary>
    /// helper for checking if the player has inventory space
    /// </summary>
    public bool CanTakeItem()
    {
        return _heldItems.Count < 2;
    }

    /// <summary>
    /// helper for if the player has an item
    /// </summary>
    public bool HasItem()
    {
        return _heldItems.Count > 0;
    }
    /// <summary>
    /// Take an item from something
    /// </summary>
    /// <param name="item">item to take</param>
    /// <returns>false if the inventory is full</returns>
    public bool TryTakeItem(GrabItem item)
    {
        if (_heldItems.Count < 2)
        {
            _heldItems.Enqueue(item);
            item.transform.SetParent(_heldItems.Count == 2 ? _slot2 : _slot1, false);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gives up an item, 
    /// its up to the taker to fix parentage, might want to unparent on this end if random dropping gets allowed
    /// </summary>
    /// <returns></returns>
    public GrabItem PlaceItem()
    {
        if (_heldItems.Count > 0)
        {
            var item = _heldItems.Dequeue();
            // shift the visual position of the 2nd item if it exists
            if (_heldItems.Count != 0)
            {
                _heldItems.Peek().transform.SetParent(_slot1, false);
            }
            return item;
        }
        return null;
    }

    /// <summary>
    /// locks the player in place
    /// </summary>
    public void LockPlayer()
    {
        _positionLocked = true;
    }

    /// <summary>
    /// releases the player
    /// </summary>
    public void UnlockPlayer()
    {
        _positionLocked = false;
    }

    /// <summary>
    /// checks if the player can move
    /// </summary>
    public bool CanMove()
    {
        return !_positionLocked;
    }

    /// <summary>
    /// call from the controller class to interact if able
    /// </summary>
    public void TryInteract()
    {
        if (_availableInteraction != null)
        {
            _availableInteraction.Interact(this);
        }
    }

    /// <summary>
    /// sets the available interaction on trigger enter
    /// </summary>
    void OnTriggerEnter2D(Collider2D col)
    {
        // this is heavily over simplified for the prototype, the only triggers are interactables
        // and they are spaced apart enough so that the player can't interact with 2 at once
        // normally we'd want to make sure the trigger is what we think it is and we'd have to handle
        // multiple interactables in range, which could be done with a facing direction combined with 
        // whichever interactable we are closest to the center of
        _availableInteraction = col.GetComponent<Interactable>();
    }

    /// <summary>
    /// unsets the available interaction on trigger exit
    /// </summary>
    void OnTriggerxit2D(Collider2D col)
    {
        _availableInteraction = null;
    }
}
