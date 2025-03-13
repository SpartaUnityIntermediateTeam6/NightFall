using UnityEngine;

[SelectionBase]
public class TestPlayer : MonoBehaviour, IDamageable
{
    public float hp = 10;

    private void Awake()
    {
        TestManager.Instance.player = transform;
    }

    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(hp - damage, 0);
        Debug.Log($"플레이어 공격. 남은 체력: {hp}");
        if (hp <= 0) Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
