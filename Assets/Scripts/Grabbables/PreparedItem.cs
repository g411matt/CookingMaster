using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An ingredient or multiple ingredients that have been processed/combined together
/// Can be held by the player and placed, served, or trashed.
/// 
/// FUTURE: If additional processing of an ingredient or mixture of ingredients is added then maybe adjust inheritance to inherit ingredient. 
/// Also additional subclassing to address different kinds of combinations that can act differently may be needed
/// </summary>
public class PreparedItem : GrabItem
{
    [SerializeField]
    private Text _text = null;
    private List<Ingredient.Type> _contents = new List<Ingredient.Type>();
    
    public void AddIngredient(Ingredient ingredient)
    {
        _contents.Add(ingredient.IngredType);
        Destroy(ingredient.gameObject);
        UpdateText();
    }

    public void AddPreparedItem(PreparedItem other)
    {
        foreach (var type in other._contents)
        {
            this._contents.Add(type);
        }
        Destroy(other.gameObject);
        UpdateText();
    }

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
