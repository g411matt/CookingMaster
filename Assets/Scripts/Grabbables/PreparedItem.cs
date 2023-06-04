using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An ingredient or multiple ingredients that have been processed/combined together
/// Can be held by the player and placed, served, or trashed.
/// Represented by '[A,B]' notation for the prototype for ease of identification in a more final presentation 
/// it would have icons for processed and unprocessed ingredients to work with as well as its own icon probably
/// 
/// FUTURE: If additional processing of an ingredient or mixture of ingredients is added then maybe adjust inheritance to inherit ingredient. 
/// Also additional subclassing to address different kinds of combinations that can act differently may be needed
/// </summary>
public class PreparedItem : GrabItem
{
    /// <summary>
    /// Reference to the text visual
    /// </summary>
    [SerializeField]
    private Text _text = null;
    /// <summary>
    /// list of ingredients in the item
    /// </summary>
    private List<Ingredient.Type> _contents = new List<Ingredient.Type>();
    
    /// <summary>
    /// add an ingredient to the item and destroy the ingredient
    /// </summary>
    public void AddIngredient(Ingredient ingredient)
    {
        _contents.Add(ingredient.IngredType);
        _contents.Sort();
        Destroy(ingredient.gameObject);
        UpdateText();
    }

    /// <summary>
    /// add a list of ingredients (enum) to item, mainly for making a customer order
    /// </summary>
    public void AddIngredients(params Ingredient.Type[] ingredients)
    {
        _contents.AddRange(ingredients);
        _contents.Sort();
        UpdateText();
    }

    /// <summary>
    /// combined two prepared items together, destroys the parameter item
    /// </summary>
    public void AddPreparedItem(PreparedItem other)
    {
        foreach (var type in other._contents)
        {
            this._contents.Add(type);
        }
        _contents.Sort();
        Destroy(other.gameObject);
        UpdateText();
    }

    /// <summary>
    /// empties the item contents, primarily for customer order displays
    /// </summary>
    public void Clear()
    {
        _contents.Clear();
        _text.text = "[]";
    }

    /// <summary>
    /// compares two prepared items to see if they match, primarily for serving
    /// </summary>
    public bool IsMatch(PreparedItem other)
    {
        // contents are always sorted so we can just loop both lists
        if (_contents.Count == other._contents.Count)
        {
            for(int i = 0; i < _contents.Count; i++)
            {
                if (_contents[i] != other._contents[i])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// update the text object that represents the item contents
    /// </summary>
    private void UpdateText()
    {
        string text = "[";
        for (int i = 0; i < _contents.Count; i++)
        {
            text += _contents[i].ToString();
            if (i != _contents.Count - 1)
            {
                text += ",";
            }
        }
        text += "]";
        _text.text = text;
    }
}
