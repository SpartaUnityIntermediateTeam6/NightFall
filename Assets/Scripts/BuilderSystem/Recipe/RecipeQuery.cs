using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeQuery
{
    public readonly RecipeData recipeData;
    public readonly Action queryEvent = delegate { };

    public RecipeQuery(RecipeData data, Action queryEvent)
    {
        recipeData = data;
        this.queryEvent += queryEvent;
    }
}
