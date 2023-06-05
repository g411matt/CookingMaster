using UnityEngine;

/// <summary>
/// Creates a designated ingredient and gives it to the player
/// future: pool ingredients instead of creating and destroying them
/// </summary>
public class Dispenser : Interactable
{
    /// <summary>
    /// prefab for the ingredient, more for the future than the prototype since the ingredient is just a text box
    /// </summary>
    [SerializeField]
    private Ingredient _ingredientPrefab = null;

    void Awake()
    {
        // turn off the dispenser if it doesn't have an ingredient
        if(_ingredientPrefab == null)
        {
            Debug.LogWarning("Disabling Dispenser without an ingredient");
            gameObject.SetActive(false);
            return;
        }
        // a visual aid for the player, does nothing else
        GameObject.Instantiate<GrabItem>(_ingredientPrefab, transform, false);
    }

    /// <summary>
    /// Give the player an ingredient if they can hold it
    /// </summary>
    public override void Interact(Player player)
    {
        if (player.CanTakeItem())
        {
            var ingred = GameObject.Instantiate<GrabItem>(_ingredientPrefab, player.transform, false);
            player.TryTakeItem(ingred);
        }
       
    }
}
