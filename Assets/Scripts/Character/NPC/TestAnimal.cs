using UnityEngine;

public class TestAnimal : NPC
{
    protected override void SetInitState()
    {
        SetState(new WanderingState(this));
    }
}
