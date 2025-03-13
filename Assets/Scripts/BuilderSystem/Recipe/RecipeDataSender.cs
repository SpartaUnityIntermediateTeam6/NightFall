using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeDataSender
{
    public readonly RecipeData recipeData;
    public readonly Action queryEvent = delegate { };

    public RecipeDataSender(RecipeData data, Action queryEvent)
    {
        recipeData = data;
        this.queryEvent += queryEvent;
    }
}
