using UnityEngine;

[SelectionBase]
public class TestBeacon : MonoBehaviour, IDamageable
{
    public float hp = 10;
    private float _maxHP;

    [Header("Floating")]
    [SerializeField] private Transform cone;
    public float floatingSpeed;
    public float floatingHeight;
    public float rotateSpeed;
    private Vector3 _startPos;

    private void Awake()
    {
        TestManager.Instance.beacon = this;

        _maxHP = hp;

        cone = transform.GetChild(0);
        _startPos = cone.position;
    }

    private void Update()
    {
        float floatY = _startPos.y + (Mathf.Sin(Time.time * floatingSpeed) + 1) * floatingHeight;
        cone.position = new Vector3(_startPos.x, floatY, _startPos.z);

        cone.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, _maxHP);
    }

    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(hp - damage, 0);
        Debug.Log($"비콘 공격. 남은 체력: {hp}");
        if (hp <= 0) Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
