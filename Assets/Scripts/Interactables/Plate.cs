using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holder to place held items down temporarily
/// </summary>
public class Plate : Interactable
{
    /// <summary>
    /// Currently held item
    /// </summary>
    private GrabItem _heldItem = null;

    /// <summary>
    /// Will either place an item on the plate or remove it depending on if the plate has anything
    /// </summary>
    public override void Interact(Player player)
    {
        if (_heldItem == null)
        {
            if (player.HasItem())
            {
                _heldItem = player.PlaceItem();
            }
        }
        else if (player.CanTakeItem())
        {
            if (player.TryTakeItem(_heldItem))
            {
                _heldItem = null;
            }
        }
    }
}
