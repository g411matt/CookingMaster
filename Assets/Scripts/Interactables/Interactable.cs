using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objects the player can interact with
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(Player player);
}
