using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;
using System;

public class FixedBuilderPlatform : MonoBehaviour, IInteractable<PlayerSample>
{
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private RecipeData recipeData;
    [SerializeField] private RecipeEvent recipeEvent;
    [SerializeField] private LayerMask targetLayers;

    private IBuilderStrategy _builderStrategy;

    void Awake() => _builderStrategy = new FixedPositionBuilder(gameObject, targetLayers);

    public bool Build()
    {
        if (_builderStrategy.CanBuild(buildingPrefab))
        {
            //인벤토리에서 체크하고 가져올때 FuncPredicate 람다식만 수정
            _builderStrategy.Build(buildingPrefab, new FuncPredicate(() => true));

            return true;
        }

        return false;
    }

    public void Interaction(PlayerSample vistor)
    {
        recipeEvent?.Raise(new RecipeQuery(recipeData, () => Build()));
    }
}