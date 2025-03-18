using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public abstract class NPC : Poolable, IDamageable
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    public GameObject[] dropOnDead;
    public float dropVerticalRange;
    public float dropHorizontalRange;
    public float dropForce;

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
    [HideInInspector] public float zOffset;
    public float attackRate;
    public float attackDamage;
    [HideInInspector] public Transform beaconTarget;

    public Animator animator;
    //private MeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        //meshRenderers = GetComponentsInChildren<MeshRenderer>();
        if(animator == null) animator = GetComponent<Animator>();

        zOffset = GetComponent<Collider>().bounds.extents.z;
    }

    protected virtual void Start()
    {
        SetInitState();
        //Dead();
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
        Debug.Log($"대미지 {damage} 입음: {name}");
        health = Mathf.Max(health - damage, 0);

        if (health <= 0) Dead();
    }

    [ContextMenu("Dead")]
    public void Dead()
    {
        if (curState is not DeadState)
        {
            SetState(new DeadState(this));
            animator.SetTrigger("dead");
            Invoke(nameof(DropPrefab), 1);
        }
    }

    public void DropPrefab()
    {
        foreach (GameObject go in dropOnDead)
        {
            ExplodeDrop(go);
        }

        GameManager.Instance.poolManager.Release(this);
    }

    void ExplodeDrop(GameObject go)
    {
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Abs(randomDir.y);

        Poolable poolable = GameManager.Instance.poolManager.Get(go);
        poolable.transform.position = transform.position
            + Vector3.up * Random.Range(1f, 1f + dropVerticalRange)
            + Vector3.forward * Random.Range(-dropHorizontalRange, dropHorizontalRange)
            + Vector3.right * Random.Range(-dropHorizontalRange, dropHorizontalRange);
        poolable.transform.rotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
        poolable.GetComponent<Rigidbody>().AddForce(randomDir * dropForce, ForceMode.Impulse);
    }
}
