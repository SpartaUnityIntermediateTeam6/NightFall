using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;

public class FixedBuilderPlatform : MonoBehaviour, IInteractable<PlayerSample>
{
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private RecipeData recipeData;
    [SerializeField] private RecipeEvent recipeEvent;
    [SerializeField] private float buildTime;
    [SerializeField] private LayerMask targetLayers;

    private IBuilderStrategy _builderStrategy;

    void Awake() => _builderStrategy = new FixedPositionBuilder(gameObject, targetLayers);

    public bool Build()
    {
        if (_builderStrategy.CanBuild(buildingPrefab))
        {
            _builderStrategy.Build(buildingPrefab, new FuncPredicate(() => true));

            return true;
        }

        return false;
    }

    public void Interaction(PlayerSample vistor)
    {
        recipeEvent?.Raise(recipeData);

        //Build();
    }
}
