using UnityEngine;

public class Enemy : NPC
{
    protected override void SetInitState()
    {
        beaconTarget = TestManager.Instance.beacon.transform;
        SetState(new AttackingState(this));
    }
}
