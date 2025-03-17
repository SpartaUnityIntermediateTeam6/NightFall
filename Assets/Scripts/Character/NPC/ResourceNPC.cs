using UnityEngine;

public class ResourceNPC : Poolable, IDamageable
{
    public float health;
    public GameObject[] dropOnDead;
    public float dropVerticalRange;
    public float dropHorizontalRange;
    public float dropForce;

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);

        if (health <= 0) Dead();
    }

    [ContextMenu("Dead")]
    public void Dead()
    {
        DropPrefab();

        Destroy(gameObject);
    }

    public void DropPrefab()
    {
        foreach (GameObject go in dropOnDead)
        {
            ExplodeDrop(go);
        }
    }

    void ExplodeDrop(GameObject go)
    {
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Abs(randomDir.y);

        Poolable poolable = TestManager.Instance.poolManager.Get(go);
        poolable.transform.position = transform.position
            + Vector3.up * Random.Range(1f, 1f + dropVerticalRange)
            + Vector3.forward * Random.Range(-dropHorizontalRange, dropHorizontalRange)
            + Vector3.right * Random.Range(-dropHorizontalRange, dropHorizontalRange);
        poolable.transform.rotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
        poolable.GetComponent<Rigidbody>().AddForce(randomDir * dropForce, ForceMode.Impulse);
    }
}
