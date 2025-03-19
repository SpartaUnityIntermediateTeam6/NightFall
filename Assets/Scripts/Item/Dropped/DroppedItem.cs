using UnityEngine;

public class DroppedItem : Poolable, IVisitor
{
    public ItemData ItemData;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<IVisitable>()?.Accept(this);
        }
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            if (player.Inventory.Add(ItemData) == 0)
            {
                GameManager.Instance.poolManager.Release(this);
            }
        }
    }

    public void Leave<T>(T visitable) where T : Component, IVisitable { }
}
