using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;
using System;

public class FixedBuilderPlatform : MonoBehaviour, IInteractable<TPSCharacterController>
{
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private RecipeData recipeData;
    [SerializeField] private RecipeGameEvent recipeEvent;
    [SerializeField] private LayerMask targetLayers;

    private IBuilderStrategy _builderStrategy;

    void Awake() => _builderStrategy = new FixedPositionBuilder(gameObject, targetLayers);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interaction(null);
        }
    }

    //*Sample, inventory or player refernce
    public void TryBuild(TPSCharacterController player)
    {
        if (_builderStrategy.CanBuild(buildingPrefab))
        {
            //인벤토리에서 체크하고 가져올때 FuncPredicate 람다식만 수정 EX/ 재료 충족하면 return true
            
            _builderStrategy.Build(buildingPrefab, new FuncPredicate(CanBuild));
        }
    }

    private bool CanBuild()
    {
        //recipeData

        return true;
    }

    public void Interaction(TPSCharacterController vistor)
    {
        //UI 버튼에 이벤트를 전달. 건물 지어졌으면 return하는 코드 추가
        recipeEvent?.Raise(new RecipeDataSender(recipeData, () => TryBuild(vistor)));
    }
}