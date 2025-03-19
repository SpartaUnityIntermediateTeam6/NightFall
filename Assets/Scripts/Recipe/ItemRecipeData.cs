using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Recipe/Item")]
public class ItemRecipeData : RecipeData<ItemRecipe>
{
    public FoodItemData resultItem;
}

[Serializable]
public class ItemRecipe : Recipe<CountableItemData>
{

}