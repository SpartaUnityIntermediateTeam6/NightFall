using UnityEngine;

public class Animal : NPC
{
    protected override void SetInitState()
    {
        SetState(new WanderingState(this));
    }
}
