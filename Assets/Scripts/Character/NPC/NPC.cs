using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    //public ItemData[] dropOnDead;

    [Header("AI")]
    [HideInInspector] public NavMeshAgent agent;
    private AIState curState;
    public float detectDistance;
    public bool isEnemy;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Attacking")]
    [HideInInspector] public Transform beaconTarget;
    //[HideInInspector] public Transform playerTarget;
    public float attackRange;
    public float attackRate;

    //private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //meshRenderers = GetComponentsInChildren<MeshRenderer>();
        agent.speed = walkSpeed;
    }

    private void Start()
    {
        if (isEnemy)
        {
            beaconTarget = TestManager.Instance.beacon.transform;
            SetState(new AttackingState(this));
        }
        else SetState(new WanderingState(this));
    }

    private void FixedUpdate()
    {
        if(curState != null) curState.FixedUpdateState();
    }

    public void SetState(AIState state)
    {
        if(curState != null) curState.ExitState();

        curState = state;
        curState.EnterState();
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
    }
}
