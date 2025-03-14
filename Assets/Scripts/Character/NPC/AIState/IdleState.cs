using UnityEngine;

public class IdleState : AIState
{
    public IdleState(NPC npc) : base(npc) { }
    public override void EnterState()
    {
        npc.agent.isStopped = true;
    }

    public override void ExitState()
    {
        npc.agent.isStopped = false;
    }

    public override void FixedUpdateState()
    {
        
    }
}
