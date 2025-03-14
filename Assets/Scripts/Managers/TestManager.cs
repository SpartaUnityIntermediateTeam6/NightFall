using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    public TestBeacon beacon;
    public Transform player;

    public PoolManager poolManager = new();

    protected override void Awake()
    {
        isGlobal = false;

        base.Awake();

        poolManager.Init();
    }
}
