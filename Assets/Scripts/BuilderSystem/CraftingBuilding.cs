using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBuilding : Building
{
    [Header("Crafing")]
    [SerializeField] private List<ItemRecipeData> itemRecipeDates = new();

    protected override void Interaction()
    {
        if (_playerCache == null)
            return;

        EventBus.Call(new CraftingInteractionEvent(itemRecipeDates, _playerCache.Inventory, TryCrafting));
    }
    
    private void TryCrafting(int index)
    {
        Debug.Log("Try " + index);
    }
}

public class CraftingInteractionEvent : IGameEvent
{
    public readonly List<ItemRecipeData> recipes;
    public readonly Inventory inventory;
    public readonly Action<int> onButtonEvent = delegate { };

    public CraftingInteractionEvent(List<ItemRecipeData> recipes, Inventory inventory, Action<int> onButtonEvent)
    {
        this.recipes = recipes;
        this.inventory = inventory;
        this.onButtonEvent = onButtonEvent;
    }
}