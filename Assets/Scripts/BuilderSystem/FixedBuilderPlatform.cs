using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;
using System;

public class FixedBuilderPlatform : MonoBehaviour, IVisitor
{
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private BuildRecipeData recipeData;
    [Header("SO Events")]
    [SerializeField] private RecipeGameEvent recipeEventChannel;
    [SerializeField] private InventoryGameEvent inventoryEventChannel;
    [SerializeField] private UnityActionGameEvent buildUIEventChannel;

    [Header("Layers")]
    [SerializeField] private LayerMask targetLayers;

    private IBuilderStrategy _builderStrategy;
    private PlayerController _playerCache;

    void Awake() => _builderStrategy = new FixedPositionBuilder(gameObject, targetLayers);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //Interaction(null);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IVisitable>()?.Accept(this);
    }

    void OnTriggerExit(Collider other)
    {
        other.GetComponent<IVisitable>()?.Cancel(this);
    }

    public void TryBuild()
    {
        if (_builderStrategy.CanBuild(buildingPrefab) && _playerCache != null)
        {
            var inventory = _playerCache.Inventory;

            foreach (var iter in recipeData.dates)
            {
                if (inventory.GetTotalAmount(iter.item) < iter.requiredAmount)
                    return;
            }

            recipeData.dates.ForEach(d => inventory.TryConsumeItem(d.item, d.requiredAmount));
            _builderStrategy.Build(buildingPrefab);
        }
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent += Interaction;
            _playerCache = player;
        }
    }

    public void Leave<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent -= Interaction;
            _playerCache = null;
            recipeEventChannel?.Raise(null);
        }
    }

    public void Interaction()
    {
        if (_playerCache == null)
            return;

        buildUIEventChannel?.Raise(TryBuild);
        recipeEventChannel?.Raise(recipeData);
        inventoryEventChannel?.Raise(_playerCache.Inventory);
    }
}