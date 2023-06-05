using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable for serving a customer, will only accept items if there's a customer and will pass plated items to separate customer system to handle scoring
/// </summary>
public class CustomerSeat : Interactable
{
    /// <summary>
    /// Customer reference, the order and progress bar should be parented under it
    /// </summary>
    [SerializeField]
    private GameObject _customer = null;

    /// <summary>
    /// Order representation for the customer
    /// </summary>
    [SerializeField]
    private PreparedItem _order = null;

    /// <summary>
    /// Progress bar for the customer wait time
    /// </summary>
    [SerializeField]
    private ProgressBar _progressBar = null;

    /// <summary>
    /// Reference back to the CustomerManager, set by manager
    /// </summary>
    private CustomerManager _manager = null;

    /// <summary>
    /// whether or not a customer is waiting
    /// </summary>
    private bool _isWaiting = false;
    /// <summary>
    /// total length customer will wait in seconds for the progress bar
    /// </summary>
    private float _initialWait = 0;
    /// <summary>
    /// time remaining until the customer leaves in seconds, this is inaccurate when the customer is angry
    /// </summary>
    private float _waitTime = 0;
    /// <summary>
    /// how much to speed up time for an angry customer
    /// </summary>
    private float _angryMult = 1;
    /// <summary>
    /// current time speed for the customer
    /// </summary>
    private float _currentMult = 1;

    void Awake() 
    {
        _customer.SetActive(false);
        _progressBar.SetProgress(1);
    }

    /// <summary>
    /// Handles the player attempting to serve food
    /// </summary>
    public override void Interact(Player player)
    {
        // does nothing if the player has no food or there is no customer
        if (player.HasItem() && _isWaiting)
        {
            var item = player.PlaceItem();
            // if the order matches the customer leaves happy otherwise they get angry if they are already
            // player score will be adjusted one way or the other
            if (item is PreparedItem && _order.IsMatch(item as PreparedItem))
            {
                // leave happy, add score
                GameManager.Instance.AddPoints(500, player);
                RemoveCustomer();
                // if 70% time or more is left spawn a pickup
                if(_waitTime / _initialWait >= .7f)
                {
                    GameManager.Instance.SpawnPickup(player);
                }
            }
            else
            {
                // angry, lower score
                GameManager.Instance.AddPoints(-500, player);
                _currentMult = _angryMult;
            }
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// Set manager reference
    /// </summary>
    public void SetManager(CustomerManager manager)
    {
        _manager = manager;
    }

    /// <summary>
    /// Set the customer waiting to eat and start the timer
    /// </summary>
    /// <param name="ingredients">list of ingredients denoting what the customer wants</param>
    /// <param name="waitTime">how long the customer will wait</param>
    /// <param name="angryMult">how much faster the timespeed should be when the customer is angry</param>
    public void SetCustomer(Ingredient.Type[] ingredients, float waitTime, float angryMult)
    {
        _order.AddIngredients(ingredients);
        _customer.SetActive(true);
        _progressBar.SetProgress(1);
        _waitTime = waitTime;
        _initialWait = waitTime;
        _angryMult = angryMult;
        _isWaiting = true;
        _currentMult = 1;
    }

    /// <summary>
    /// Clear the customer and return the seat to the queue
    /// </summary>
    public void RemoveCustomer()
    {
        _order.Clear();
        _customer.SetActive(false);
        _manager.EmptySeat(this);
        _isWaiting = false;
    }

    void Update()
    {
        if (_isWaiting)
        {
            // multiplying the delta time by the angry multiplier speeds up how soon the customer leaves,
            // but we don't actually know long that is so if we want to display actual wait times then this will probably change
            _waitTime -= Time.deltaTime * _currentMult;
            _progressBar.SetProgress(_waitTime / _initialWait);
            if (_waitTime <= 0)
            {
                // leave, lower scores
                GameManager.Instance.AddPointsP1(-250);
                GameManager.Instance.AddPointsP2(-250);
                RemoveCustomer();
            }
        }
    }

    public override void Reset()
    {
        RemoveCustomer();
    }
}
