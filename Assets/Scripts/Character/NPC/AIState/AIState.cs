public abstract class AIState
{
    protected NPC npc;

    public AIState(NPC npc)
    {
        this.npc = npc;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void FixedUpdateState();
}
