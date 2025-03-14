using UnityEngine;

public class DroppedItem : Poolable, IVisitor
{
    public ItemData ItemData;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IVisitable>()?.Accept(this);
        }
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerController player)
        {
            //인벤토리 검사 수행
            //인벤토리 검사 완료하면
            //아이템 추가

            TestManager.Instance.poolManager.Release(this);
        }
    }

    public void Leave<T>(T visitable) where T : Component, IVisitable { }
}
