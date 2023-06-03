using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An ingredient or multiple ingredients that have been processed/combined together
/// Can be held by the player and placed, served, or trashed.
/// 
/// FUTURE: If additional processing of an ingredient or mixture of ingredients is added then maybe adjust inheritance to inherit ingredient. 
/// Also additional subclassing to address different kinds of combinations that can act differently may be needed
/// </summary>
public class PreparedItem : GrabItem
{

    private List<Ingredient.Type> _contents = new List<Ingredient.Type>();
    
}
