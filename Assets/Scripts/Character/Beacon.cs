using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Beacon : MonoBehaviour, IDamageable
{
    [SerializeField] private float _hp = 10;
    [HideInInspector] public float HP
    {
        get => _hp;
        set
        {
            _hp = Mathf.Clamp(value, 0, _maxHP);
            beaconHpEventChannel?.Raise(new BoundedValue(_hp, 0, _maxHP));
        }
    }
    private float _maxHP;

    [Header("Floating")]
    [SerializeField] private Transform cone;
    public float floatingSpeed;
    public float floatingHeight;
    public float rotateSpeed;
    private Vector3 _startPos;

    [SerializeField] private BoundedValueGameEvent beaconHpEventChannel;

    private void Awake()
    {
        GameManager.Instance.beacon = this;

        _maxHP = HP;
        beaconHpEventChannel?.Raise(new BoundedValue(_hp, 0, _maxHP));

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
        HP = Mathf.Min(HP + amount, _maxHP);
    }

    public void TakeDamage(float damage)
    {
        HP = Mathf.Max(HP - damage, 0);
        Debug.Log($"비콘 공격. 남은 체력: {HP}");
        if (HP <= 0) Dead();
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

}
