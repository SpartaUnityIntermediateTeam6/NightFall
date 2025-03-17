using UnityEngine;

[SelectionBase]
public class PassiveBuilding : Building
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
