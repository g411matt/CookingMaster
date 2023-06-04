using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages putting customers in seats and their orders
/// </summary>
public class CustomerManager : MonoBehaviour
{
    /// <summary>
    /// reference to the available seats
    /// </summary>
    [SerializeField]
    private CustomerSeat[] _customerSeats = null;

    /// <summary>
    /// time between when a seat was last filled or a new seat becomes available and when a seat can be filled
    /// could add a bit of randomness to the time to vary things, could also do fixed customer spawn times or flux spawn rates as part of level design
    /// </summary>
    [SerializeField]
    private float _timeBetweenSeating = 5;

    /// <summary>
    /// How long a customer will wait, should add variance to it in the future
    /// </summary>
    [SerializeField, Tooltip("time in seconds")]
    private float _customerWaitTime = 45;
    /// <summary>
    /// Multiplier for speeding up wait of angry customers, could add variance in the future
    /// </summary>
    [SerializeField]
    private float _angryMultiplier = 1.25f;

    /// <summary>
    /// The seat queue for customers
    /// </summary>
    private Queue<CustomerSeat> _availableSeats = new Queue<CustomerSeat>();
    /// <summary>
    /// time since last seat
    /// </summary>
    private float _seatingTime = 0;
    
    void Awake()
    {
        for(int i = 0; i < _customerSeats.Length; i++)
        {
            _availableSeats.Enqueue(_customerSeats[i]);
            _customerSeats[i].SetManager(this);
        }
    }

    /// <summary>
    /// return a seat to the queue after the customer leaves
    /// </summary>
    public void EmptySeat(CustomerSeat seat)
    {
        _availableSeats.Enqueue(seat);
    }

    void Update()
    {
        if (_availableSeats.Count > 0)
        {
            _seatingTime += Time.deltaTime;
            if (_seatingTime > _timeBetweenSeating)
            {
                var seat = _availableSeats.Dequeue();
                // customer should want something with 2-4 ingredients
                int count = Random.Range(2, 5);
                var ingreds = new List<Ingredient.Type>(count);
                for (int i = 0; i < count; i++)
                {
                    // grab 'count' number of ingredients and make sure they are unique
                    Ingredient.Type ingred;
                    do
                    {
                        ingred = (Ingredient.Type)Random.Range(0, 6);
                    } while (ingreds.Contains(ingred));
                    ingreds.Add(ingred);
                }
                // seat the customer
                seat.SetCustomer(ingreds.ToArray(), _customerWaitTime, _angryMultiplier);
                _seatingTime = 0;
            }
        }
    }
}
