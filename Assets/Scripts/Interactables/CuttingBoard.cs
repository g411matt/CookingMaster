using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Workstation used to prepare and combine ingredients, the player cannot move when "chopping"
/// </summary>
public class CuttingBoard : Interactable
{
    /// <summary>
    /// prefab for the generic prepared item, 
    /// in the future there should probably be a separate scriptable object in charge of managing prefabs for 
    /// prepared ingredient and ingredient combos, but this is enough until there are multiple prefabs and preperation methods
    /// </summary>
    [SerializeField]
    private PreparedItem _prepPrefab = null;

    [SerializeField]
    private float _chopTime = 3;

    private PreparedItem _currentPrep = null;

    private bool _inUse = false;
    private Player _currPlayer = null;
    private float _currentTime = 0;

    public override void Interact(Player player)
    {
        if(!_inUse && player.HasItem())
        {
           var item = player.PlaceItem();
           if (item is Ingredient)
           {
                _currPlayer = player;
                BeginProcessIngredient(item as Ingredient);   
           }
           else if (item is PreparedItem)
            {
                var prep = item as PreparedItem;
                if (_currentPrep == null)
                {
                    _currentPrep = prep;
                }
                else
                {
                    // add the contents of prep to _currentPrep
                }
            }
        }
    }

    private void BeginProcessIngredient(Ingredient ingredient)
    {
        if(_currentPrep == null)
        {
            _currentPrep = GameObject.Instantiate<PreparedItem>(_prepPrefab, this.transform, false);
        }
        _inUse = true;
        _currPlayer.LockPlayer();
        // add the ingredient to the prep
        _currentTime = 0;
        Destroy(ingredient.gameObject); // maybe save this and show it during chopping then get rid of it, or just show the prepared item
    }


    void Update()
    {
        if (_inUse)
        {
            // TODO: add visual progress bar
            _currentTime += Time.deltaTime;
            if (_currentTime >= _chopTime)
            {
                _currPlayer.UnlockPlayer();
                _inUse = false;
            }
        }
    }
}
