using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Input handler for the player, very basic just to get the player moving and grabbing things
/// keeping it separate from the rest of the player logic so that the input can be more flexible
/// with rebinding and gamepad support without bloating the core player functionality. 
/// </summary>
public class PlayerController : MonoBehaviour
{
    // references so we don't need an awake just to get them
    [Header("References")]
    [SerializeField]
    private Player _player = null;
    // hardcoded controls for the prototype, serialized to set differently for 2 players
    [Header("Keycodes")]
    [SerializeField]
    private KeyCode _left = KeyCode.A;
    [SerializeField]
    private KeyCode _right = KeyCode.D;
    [SerializeField]
    private KeyCode _up = KeyCode.W;
    [SerializeField]
    private KeyCode _down = KeyCode.S;
    [SerializeField]
    private KeyCode _interact = KeyCode.E;
    [SerializeField]
    private KeyCode _pause = KeyCode.Escape;


    /// <summary>
    /// Check inputs and set the move velocity
    /// </summary>
    void Update()
    {
        Vector2 moveVector = Vector2.zero;
        if (Input.GetKeyUp(_pause))
        {
            // kill move speed on pause just in case
            _player.SetMoveVelocity(moveVector);
            GameManager.Instance.Pause(Time.timeScale != 0);
            return;
        }
        if (_player.CanMove())
        {
            if (Input.GetKey(_left))
            {
                moveVector.x = -1;
            }
            else if (Input.GetKey(_right))
            {
                moveVector.x = 1;
            }

            if (Input.GetKey(_up))
            {
                moveVector.y = 1;
            }
            else if (Input.GetKey(_down))
            {
                moveVector.y = -1;
            }
        }

        _player.SetMoveVelocity(moveVector.normalized);

        if (Input.GetKeyUp(_interact))
        {
            _player.TryInteract();
        }
    }
}
