using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a designated ingredient and gives it to the player
/// </summary>
public class Dispenser : Interactable
{
    [SerializeField]
    private GrabItem _ingredientPrefab = null;

    public override void Interact(Player player)
    {
        if (_ingredientPrefab != null && player.CanTakeItem())
        {
            var ingred = GameObject.Instantiate<GrabItem>(_ingredientPrefab, player.transform, false);
            ingred.gameObject.SetActive(false);
            player.TryTakeItem(ingred);
        }
       
    }
}
