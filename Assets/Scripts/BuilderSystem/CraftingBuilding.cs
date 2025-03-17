using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBuilding : Building
{
    [Header("Crafing")]
    [SerializeField] private List<ItemRecipeData> itemRecipeDates = new();

    private bool _uiFlag = false;

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        //EventBus.Call(new CraftingInteractionEvent(itemRecipeDates, _playerCache.Inventory, TryCrafting, false));
        _uiFlag = false;
    }

    private void TryCrafting(int index)
    {
        if (index >= itemRecipeDates.Count)
            return;

        if (_playerCache != null)
        {
            var inventory = _playerCache.Inventory;

            foreach (var iter in itemRecipeDates[index].recipeDates)
            {
                if (inventory.GetTotalAmount(iter.itemData) < iter.requiredAmount)
                    return;
            }
            itemRecipeDates[index].recipeDates.ForEach(r => inventory.TryConsumeItem(r.itemData, r.requiredAmount));
            inventory.Add(itemRecipeDates[index].resultItem);
        }
    }

    protected override void Interaction()
    {
        if (_playerCache == null)
            return;

        EventBus.Call(new CraftingInteractionEvent(itemRecipeDates, _playerCache.Inventory, TryCrafting, !_uiFlag));
        _uiFlag = !_uiFlag;
    }
}

public class CraftingInteractionEvent : IGameEvent
{
    public readonly List<ItemRecipeData> recipes;
    public readonly Inventory inventory;
    public readonly Action<int> onButtonEvent = delegate { };
    public bool UIActive { get; set; }

    public CraftingInteractionEvent(List<ItemRecipeData> recipes, Inventory inventory, Action<int> onButtonEvent, bool uIActive = true)
    {
        this.recipes = recipes;
        this.inventory = inventory;
        this.onButtonEvent = onButtonEvent;
        UIActive = uIActive;
    }
}