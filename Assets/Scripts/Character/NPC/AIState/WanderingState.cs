using UnityEngine;
using UnityEngine.AI;

public class WanderingState : AIState
{
    private float waitTime;

    public WanderingState(NPC npc) : base(npc) {  }

    public override void EnterState()
    {
        npc.agent.isStopped = false;
        SetNextDestination();
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        if(!npc.agent.pathPending && npc.agent.remainingDistance <= 0.1f)
        {
            npc.animator.SetBool("isMoving", false);
            waitTime -= Time.deltaTime;
            if(waitTime <= 0f)
            {
                SetNextDestination();
            }
        }

        //if(npc.target != null)
        //{
        //    float distance = Vector3.Distance(npc.transform.position, npc.target.position);
        //    if(distance <= npc.detectDistance)
        //    {
        //        npc.SetState(new AttackingState(npc));
        //    }
        //}
    }

    void SetNextDestination()
    {
        float wanderDistance = Random.Range(npc.minWanderDistance, npc.maxWanderDistance);
        Vector3 randomDir = Random.onUnitSphere * wanderDistance + npc.transform.position;
        if(NavMesh.SamplePosition(randomDir, out NavMeshHit hit, wanderDistance, NavMesh.AllAreas))
        {
            npc.agent.SetDestination(hit.position);
        }
        npc.animator.SetBool("isMoving", true);
        waitTime = Random.Range(npc.minWanderWaitTime, npc.maxWanderWaitTime);
    }
}
