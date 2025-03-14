using UnityEngine;

public class DroppedItem : Poolable
{
    public ItemData ItemData;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) Gather();
    }

    void Gather()
    {
        TestManager.Instance.poolManager.Release(this);
        //인벤토리로 들어가는 메서드
    }
}
