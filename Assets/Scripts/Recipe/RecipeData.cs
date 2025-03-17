using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RecipeData<T> : ScriptableObject
{
    public List<T> recipeDates = new();
}

[Serializable]
public class Recipe<T> where T : ItemData
{
    public T itemData;
    public int requiredAmount;
}