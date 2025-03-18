using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackingBuilding : Building
{
    [Header("Attacking Setting")]
    [SerializeField] private float attackDelay;
    [SerializeField] private Transform attackTarget;
    [SerializeField] private float rangeRadius = 0.5f;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private RotateToTarget cannonRotate;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    private WaitForSeconds _yieldCache;

    void Awake() => _yieldCache = new WaitForSeconds(attackDelay);

    void OnEnable() => StartCoroutine(DelayAttack());

    private Transform SearchTarget()
    {
        var hit = Physics.OverlapSphere(transform.position, rangeRadius, targetLayers);

        //Enemy 레이어 따로 지정
        var closest = hit.Where(c => c.CompareTag("NPC"))
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position))
            .FirstOrDefault();

        return closest?.transform;
    }

    IEnumerator DelayAttack()
    {
        while (true)
        {
            if (attackTarget == null || !attackTarget.gameObject.activeInHierarchy || 
                Vector3.Distance(transform.position, attackTarget.transform.position) > rangeRadius)
            {
                attackTarget = SearchTarget();
            }

            if (attackTarget != null)
            {
                //Attacking
                cannonRotate?.SetTarget(attackTarget);
                Debug.Log("111111");
            }

            yield return _yieldCache;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
        Gizmos.DrawSphere(transform.position, rangeRadius);
    }
#endif
}