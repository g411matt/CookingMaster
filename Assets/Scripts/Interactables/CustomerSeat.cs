using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable for serving a customer, will only accept items if there's a customer and will pass plated items to separate customer system to handle scoring
/// TDOO: connect to customer system when built
/// </summary>
public class CustomerSeat : Interactable
{
    public override void Interact(Player player)
    {
        // tODO: customer handling;
        player.PlaceItem();
    }
}
