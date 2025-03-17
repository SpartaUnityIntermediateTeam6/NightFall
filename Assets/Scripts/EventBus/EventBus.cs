using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, Delegate> assignedEvents = new();

    public static void Call<T>(T eventData) where T : IGameEvent
    {
        if (assignedEvents.TryGetValue(eventData.GetType(), out var value))
        {
            (value as Action<T>)?.Invoke(eventData);
        }
    }

    public static void Subscribe<T>(Action<T> action) where T : IGameEvent
    {
        var type = typeof(T);

        assignedEvents[type] = assignedEvents.ContainsKey(type) ?
            Delegate.Combine(assignedEvents[type], action) : action;
    }

    public static void Unsubscribe<T>(Action<T> action) where T : IGameEvent
    {
        var type = typeof(T);

        if (assignedEvents.ContainsKey(type))
        {
            assignedEvents[type] = Delegate.Remove(assignedEvents[type], action);
        }
    }
}

public interface IGameEvent
{
}
