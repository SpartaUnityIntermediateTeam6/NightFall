using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private void Awake()
    {
        TestManager.Instance.player = transform;
    }
}
