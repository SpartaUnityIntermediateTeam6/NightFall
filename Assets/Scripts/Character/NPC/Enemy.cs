using UnityEngine;

public class Enemy : NPC
{
    protected override void SetInitState()
    {
        beaconTarget = GameManager.Instance.beacon.transform;
        SetState(new AttackingState(this));
    }
}
