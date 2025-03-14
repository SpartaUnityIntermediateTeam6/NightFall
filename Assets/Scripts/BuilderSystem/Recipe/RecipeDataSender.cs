using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeDataSender
{
    public readonly RecipeData recipeData;
    public readonly Action OnClickEvent = delegate { };

    public RecipeDataSender(RecipeData data, Action buttonEvent)
    {
        recipeData = data;
        this.OnClickEvent += buttonEvent;
    }
}
