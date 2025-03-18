using UnityEngine;

public class Projectile : Poolable
{
    [SerializeField] private float moveSpeed = 5f;

    private Transform _target;
    private float _damage;

    void Update()
    {
        if (_target == null || !_target.gameObject.activeInHierarchy)
        {
            GameManager.Instance.poolManager.Release(this);
            _target = null;
            return;
        }

        transform.position += (_target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<IDamageable>()?.TakeDamage(_damage);
            GameManager.Instance.poolManager.Release(this);
        }
    }
}
