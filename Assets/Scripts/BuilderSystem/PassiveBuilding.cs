using UnityEngine;

[SelectionBase]
public class PassiveBuilding : Building
{
    public float damage;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
