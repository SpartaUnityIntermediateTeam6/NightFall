using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "Recipe/Building")]
public class BuildRecipeData : ScriptableObject
{
    public List<BuildingRecipe> dates = new();
}


[Serializable]
public class BuildingRecipe
{
    public BuildingMaterialItemData item;
    public int requiredAmount;
}
