using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeDataSender
{
    public BuildRecipeData RecipeData { get; }
    public Action OnClickEvent = delegate { };
    public Inventory Inventory { get; }

    public RecipeDataSender(BuildRecipeData data, Inventory inventory, Action buttonEvent)
    {
        RecipeData = data;
        OnClickEvent += buttonEvent;
        Inventory = inventory;
    }
}
