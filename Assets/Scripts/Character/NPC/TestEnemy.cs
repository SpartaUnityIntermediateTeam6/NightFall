using UnityEngine;

public class TestEnemy : NPC
{
    protected override void SetInitState()
    {
        beaconTarget = TestManager.Instance.beacon.transform;
        SetState(new AttackingState(this));
    }
}
