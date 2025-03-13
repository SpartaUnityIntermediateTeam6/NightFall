using UnityEngine;

public class Beacon : MonoBehaviour, IDamageable
{
    private float _hp = 10;
    private float _maxHP;

    private void Awake()
    {
        TestManager.Instance.beacon = this;

        _maxHP = _hp;
    }

    public void Heal(int amount)
    {
        _hp = Mathf.Min(_hp + amount, _maxHP);
    }

    public void TakeDamage(float damage)
    {
        _hp = Mathf.Max(_hp - damage, 0);
        if (_hp <= 0) Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
