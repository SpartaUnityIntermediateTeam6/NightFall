using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "SO/Recipe")]
public class RecipeData : ScriptableObject
{
    public List<Recipe> dates = new();
}


[Serializable]
public class Recipe
{
    public ScriptableObject item;
}
