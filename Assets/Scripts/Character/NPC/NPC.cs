using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public abstract class NPC : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    public GameObject[] dropOnDead;

    [Header("AI")]
    [HideInInspector] public NavMeshAgent agent;
    private AIState curState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Attacking")]
    public float detectDistance;
    public float attackRange;
    public float attackRate;
    public float attackDamage;
    [HideInInspector] public Transform beaconTarget;
    //[HideInInspector] public Transform playerTarget;

    //private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //meshRenderers = GetComponentsInChildren<MeshRenderer>();
        agent.speed = walkSpeed;
    }

    protected virtual void Start()
    {
        SetInitState();
    }

    private void FixedUpdate()
    {
        if(curState != null) curState.FixedUpdateState();
    }

    protected abstract void SetInitState();

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

    public void Dead()
    {
        foreach(GameObject go in dropOnDead)
        {
            Instantiate(go, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
