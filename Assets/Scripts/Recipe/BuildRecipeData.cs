using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Recipe/Building")]
public class BuildRecipeData : RecipeData<BuildingRecipe>
{
}

[Serializable]
public class BuildingRecipe : Recipe<BuildingMaterialItemData>
{
}
