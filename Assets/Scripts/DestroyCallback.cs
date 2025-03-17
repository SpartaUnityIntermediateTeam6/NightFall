using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCallback : MonoBehaviour
{
    public event Action OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
