using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent<T> : ScriptableObject
{
    private readonly List<IGameEventListener<T>> _listeners = new();

    public void Raise(T data)
    {
        foreach (var iter in _listeners)
        {
            iter.OnEventRaised(data);
        }
    }

    public void RegisterListener(IGameEventListener<T> listener) => _listeners.Add(listener);
    public void DeregisterListener(IGameEventListener<T> listener) => _listeners.Remove(listener);
}
