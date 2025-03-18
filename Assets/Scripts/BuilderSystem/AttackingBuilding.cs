using System.Collections;
using System.Linq;
using UnityEngine;

public class AttackingBuilding : Building
{
    [Header("Attacking Setting")]
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackPower = 5f;
    [SerializeField] private float rangeRadius = 0.5f;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private RotateToTarget cannonRotate;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

    [SerializeField]
    private Transform _attackTarget;
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
            if (_attackTarget == null || !_attackTarget.gameObject.activeInHierarchy || 
                Vector3.Distance(transform.position, _attackTarget.transform.position) > rangeRadius)
            {
                _attackTarget = SearchTarget();
            }

            if (_attackTarget != null)
            {
                //Attacking
                cannonRotate?.SetTarget(_attackTarget);

                var projectile = GameManager.Instance.poolManager.Get(projectilePrefab).GetComponent<Projectile>();
                projectile.transform.position = cannonRotate.transform.position;
                projectile?.SetTarget(_attackTarget);
                projectile?.SetDamage(attackPower);
            }

            Debug.Log(_attackTarget);

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