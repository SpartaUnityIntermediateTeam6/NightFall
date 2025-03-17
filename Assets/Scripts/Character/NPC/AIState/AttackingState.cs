using UnityEngine;

public class AttackingState : AIState
{
    float lastAttackTime;

    public AttackingState(NPC npc) : base(npc) {  }

    public override void EnterState()
    {
        npc.agent.isStopped = false;
        lastAttackTime = Time.time;
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        Transform curTarget = npc.beaconTarget;

        float beaconDistance = Vector3.Distance(npc.transform.position, npc.beaconTarget.position);
        float playerDistance = Vector3.Distance(npc.transform.position, TestManager.Instance.player.position);
        if (playerDistance < beaconDistance && playerDistance <= npc.detectDistance)
        {
            curTarget = TestManager.Instance.player;
        }

        npc.agent.SetDestination(curTarget.position);
        float distance = Vector3.Distance(npc.transform.position, curTarget.position);
        
        if (distance > npc.attackRange + npc.zOffset)
        {
            npc.agent.isStopped = false;
            npc.animator.SetBool("isMoving", true);
        }
        else
        {
            npc.agent.isStopped = true;
            npc.agent.velocity = Vector3.zero;
            npc.animator.SetBool("isMoving", false);
            Quaternion lookRotation = Quaternion.LookRotation(curTarget.position - npc.transform.position);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, lookRotation, 0.1f);
            Attack(curTarget);
        }
    }

    void Attack(Transform target)
    {
        if (Time.time - lastAttackTime <= npc.attackRate) return;

        npc.animator.SetTrigger("attack");

        lastAttackTime = Time.time;
        target.GetComponent<IDamageable>()?.TakeDamage(npc.attackDamage);
    }
}
