using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingSystem;

public class Building : MonoBehaviour, IVisitor
{
    [SerializeField] protected BuildRecipeData recipeData;
    [SerializeField] protected string buildingName;
    [SerializeField, TextArea] protected string buildingDescription;

    void OnTriggerEnter(Collider other) => other.GetComponent<IVisitable>()?.Accept(this);
    void OnTriggerExit(Collider other) => other.GetComponent<IVisitable>()?.Cancel(this);

    public void Leave<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent += Interaction;
        }
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            player.OnInteractionEvent -= Interaction;
        }
    }

    private void Interaction()
    {

    }
}
