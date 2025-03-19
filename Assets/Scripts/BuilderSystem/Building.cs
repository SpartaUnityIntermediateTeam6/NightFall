using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;

public class Building : MonoBehaviour, IVisitor
{
    [SerializeField] protected BuildRecipeData recipeData;
    [SerializeField] protected string buildingName;
    [SerializeField, TextArea] protected string buildingDescription;

    protected PlayerController _playerCache;

    public BuildRecipeData RecipeData => recipeData;
    public string BuildingName => buildingName;
    public string BuildingDescription => buildingDescription;

    protected virtual void OnTriggerEnter(Collider other) => other.GetComponent<IVisitable>()?.Accept(this);
    protected virtual void OnTriggerExit(Collider other) => other.GetComponent<IVisitable>()?.Cancel(this);

    public virtual void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent += Interaction;
            _playerCache = player;
        }
    }

    public virtual void Leave<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent -= Interaction;
            _playerCache = null;
        }
    }

    protected virtual void Interaction()
    {
        
    }
}
