﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ingredient to be processed. Is produced by a dispenser and can be held by a player.
/// Additional flags may be needed if more than one processing method is added
/// </summary>
public class Ingredient : GrabItem
{
    public enum Type 
    {
        A,
        B,
        C,
        D,
        E,
        F
    }

    [SerializeField]
    private Type _type = Type.A;

}
