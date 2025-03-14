using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;
using System;

public class FixedBuilderPlatform : MonoBehaviour, IVisitor
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
            //Interaction(null);
        }
    }

    //*Sample, inventory or player refernce
    public void TryBuild()
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

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent += Interaction;
        }
    }

    public void Leave<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent -= Interaction;
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

    public void Interaction()
    {
        recipeEvent?.Raise(new RecipeDataSender(recipeData, TryBuild));
        //UI 버튼에 이벤트를 전달. 건물 지어졌으면 return하는 코드 추가
        //recipeEvent?.Raise(new RecipeDataSender(recipeData, () => TryBuild(vistor)));
    }
}