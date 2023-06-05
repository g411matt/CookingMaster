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

    /// <summary>
    /// progress bar for chopping time
    /// </summary>
    [SerializeField]
    private ProgressBar _progressBar = null;

    /// <summary>
    /// Time to chop ingredients in seconds
    /// </summary>
    [SerializeField, Tooltip("Chop time in seconds")]
    private float _chopTime = 3;

    /// <summary>
    /// holder for the current processed item
    /// </summary>
    private PreparedItem _currentPrep = null;

    /// <summary>
    /// whether or not the board is chopping, board can't be interacted with while true
    /// </summary>
    private bool _inUse = false;
    /// <summary>
    /// player reference for lock release during chopping
    /// </summary>
    private Player _currPlayer = null;
    /// <summary>
    /// time spent chopping the current ingredient
    /// </summary>
    private float _currentTime = 0;

    /// <summary>
    /// Handles interaction with a player, if the player has an item it takes it, otherwise it will give the player a prepared item if it has one
    /// If given an ingredient, the player is locked in place while the ingredient is chopped, everything place on the same board will be combined
    /// </summary>
    public override void Interact(Player player)
    {
        if (!_inUse)
        {
            if (player.HasItem())
            {
                var item = player.PlaceItem();
                // if given an ingredient make a prepared item if there isn't one, add the ingredient to it, then lock the player
                if (item is Ingredient)
                {
                    _currPlayer = player;
                    var ingredient = item as Ingredient;
                    if (_currentPrep == null)
                    {
                        _currentPrep = GameObject.Instantiate<PreparedItem>(_prepPrefab, this.transform, false);
                    }
                    _currentPrep.AddIngredient(ingredient);
                    _inUse = true;
                    _currPlayer.LockPlayer();
                    _currentTime = 0;
                    _progressBar.gameObject.SetActive(true);
                    _progressBar.SetProgress(0);
                }
                // if given a prepared item, combine them or just take it if there isn't one already
                else if (item is PreparedItem)
                {
                    var prep = item as PreparedItem;
                    if (_currentPrep == null)
                    {
                        _currentPrep = prep;
                        _currentPrep.transform.SetParent(transform, false);
                    }
                    else
                    {
                        _currentPrep.AddPreparedItem(prep);
                    }
                }
            }
            // if the player is empty handed give them the prepared item if it exists
            else if(_currentPrep != null)
            {
                player.TryTakeItem(_currentPrep);
                _currentPrep = null;
            }
        }
    }

    /// <summary>
    /// used to track time for ingredient chopping
    /// </summary>
    void Update()
    {
        if (_inUse)
        {
            // TODO: add visual progress bar
            _currentTime += Time.deltaTime;
            _progressBar.SetProgress(_currentTime / _chopTime);
            if (_currentTime >= _chopTime)
            {
                _currPlayer.UnlockPlayer();
                _inUse = false;
                _progressBar.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// reset all flags and get rid of anything the board is holding
    /// </summary>
    public override void Reset()
    {
        _inUse = false;
        _currPlayer = null;
        _currentTime = 0;
        _progressBar.gameObject.SetActive(false);
        if (_currentPrep != null)
        {
            Destroy(_currentPrep.gameObject);
            _currentPrep = null;
        }
    }
}
