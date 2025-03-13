using UnityEngine;

public class TestManager : Singleton<TestManager>
{
    public TestBeacon beacon;
    public Transform player;

    protected override void Awake()
    {
        isGlobal = false;

        base.Awake();
    }
}
