using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;

public class FixedBuilderPlatform : MonoBehaviour
{
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private float buildTime;
    [SerializeField] private LayerMask targetLayers;

    private IBuilderStrategy _builderStrategy;

    void Awake()
    {
        _builderStrategy = new FixedPositionBuilder(gameObject, targetLayers);
    }

    //Sample Code
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(Build());
        }
    }

    public bool Build()
    {
        if (_builderStrategy.CanBuild(buildingPrefab))
        {
            _builderStrategy.Build(buildingPrefab, new FuncPredicate(() => true));

            return true;
        }

        return false;
    }
}
