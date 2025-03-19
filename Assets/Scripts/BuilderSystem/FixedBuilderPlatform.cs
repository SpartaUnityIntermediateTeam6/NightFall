using BuildingSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FixedBuilderPlatform : MonoBehaviour, IVisitor
{
    [SerializeField] private BoolGameEvent uiEventChannel;
    [SerializeField] private List<Building> buildingPrefabs = new();
    [SerializeField] private GameObject buildDistanceGameObject;

    [Header("Layers")]
    [SerializeField] private LayerMask targetLayers;

    private IBuilderStrategy _builderStrategy;
    private PlayerController _playerCache;
    private bool _canBuild = true;

    void Awake() => _builderStrategy = new FixedPositionBuilder(gameObject, targetLayers);

    void OnTriggerEnter(Collider other) => other.GetComponent<IVisitable>()?.Accept(this);

    void OnTriggerExit(Collider other) => other.GetComponent<IVisitable>()?.Cancel(this);

    public void TryBuild(int index)
    {
        if (index >= buildingPrefabs.Count || !_canBuild)
            return;

        if (_builderStrategy.CanBuild(buildingPrefabs[index]) && _playerCache != null)
        {
            var inventory = _playerCache.Inventory;

            foreach (var iter in buildingPrefabs[index].RecipeData.recipeDates)
            {
                if (inventory.GetTotalAmount(iter.itemData) < iter.requiredAmount)
                    return;
            }
            buildingPrefabs[index].RecipeData.recipeDates.ForEach(r => inventory.TryConsumeItem(r.itemData,
                r.requiredAmount));

            _builderStrategy.Build(buildingPrefabs[index]);
            _canBuild = false;
            uiEventChannel?.Raise(false);
            Destroy(buildDistanceGameObject);
        }
    }

    public void Interaction()
    {
        if (_playerCache == null || !_canBuild)
        {
            uiEventChannel?.Raise(false);
            return;
        }

        EventBus.Call(new BuildInteractionEvent(buildingPrefabs, _playerCache?.Inventory, TryBuild));
        uiEventChannel?.Raise(true);
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
            uiEventChannel?.Raise(false);
            player.OnInteractionEvent -= Interaction;
            _playerCache = null;
        }
    }
}

public class BuildInteractionEvent : IGameEvent
{
    public readonly List<Building> buildings = new();
    public readonly Inventory inventory;
    public readonly Action<int> onButtonEvent = delegate { };

    public BuildInteractionEvent(List<Building> buildingsInfo, Inventory inventory, Action<int> onButtonEvent)
    {
        buildings = buildingsInfo;
        this.inventory = inventory;
        this.onButtonEvent = onButtonEvent;
    }
}