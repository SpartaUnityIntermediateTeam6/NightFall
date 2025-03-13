using UnityEngine;

public class AttackingState : AIState
{
    public AttackingState(NPC npc) : base(npc) {  }

    public override void EnterState()
    {
        npc.agent.isStopped = false;
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        Transform curTarget = npc.beaconTarget;

        float playerDistance = Vector3.Distance(npc.transform.position, TestManager.Instance.player.position);
        if(playerDistance <= npc.detectDistance) curTarget = TestManager.Instance.player;
        Debug.Log(playerDistance);
        if (curTarget == null) return;

        float distance = Vector3.Distance(npc.transform.position, curTarget.position);
        if (distance > npc.attackRange) npc.agent.SetDestination(curTarget.position);
        else Attack(curTarget);
    }

    void Attack(Transform target)
    {
        if (target == npc.beaconTarget) Debug.Log("비콘 공격");
        else Debug.Log("플레이어 공격");
    }
}
