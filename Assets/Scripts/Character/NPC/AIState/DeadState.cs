using UnityEngine;

public class DeadState : AIState
{
    public DeadState(NPC npc) : base(npc) { }
    public override void EnterState()
    {
        npc.agent.isStopped = true;
        npc.agent.velocity = Vector3.zero;
    }

    public override void ExitState()
    {
        npc.agent.isStopped = false;
    }

    public override void FixedUpdateState()
    {
        
    }
}
