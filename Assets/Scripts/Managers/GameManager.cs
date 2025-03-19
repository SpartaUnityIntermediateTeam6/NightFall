using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Beacon beacon;
    public Transform player;

    public PoolManager poolManager = new();

    public SunMoonCycle sunMoonCycle = new();

    protected override void Awake()
    {
        isGlobal = false;

        base.Awake();

        poolManager.Init();
    }
}
