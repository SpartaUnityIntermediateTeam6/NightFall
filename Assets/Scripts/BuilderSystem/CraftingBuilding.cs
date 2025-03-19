using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBuilding : Building
{
    [SerializeField] private BoolGameEvent uiEventChannel;
    [Header("Crafing")]
    [SerializeField] private List<ItemRecipeData> itemRecipeDates = new();

    public override void Leave<T>(T visitable)
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent -= Interaction;
            _playerCache = null;
            uiEventChannel?.Raise(false);
        }
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

        EventBus.Call(new CraftingInteractionEvent(itemRecipeDates, _playerCache.Inventory, TryCrafting));
        uiEventChannel?.Raise(true);
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