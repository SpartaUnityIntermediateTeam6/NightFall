using UnityEngine;

[SelectionBase]
public class TestBeacon : MonoBehaviour, IDamageable
{
    public float hp = 10;
    private float _maxHP;

    private void Awake()
    {
        TestManager.Instance.beacon = this;

        _maxHP = hp;
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, _maxHP);
    }

    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(hp - damage, 0);
        Debug.Log($"���� ����. ���� ü��: {hp}");
        if (hp <= 0) Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
