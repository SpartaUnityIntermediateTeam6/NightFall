using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private static readonly Dictionary<Type, IService> _services = new();

    public static void Register<T>(T service) where T : IService
    {
        _services[typeof(T)] = service;
    }

    public static T GetService<T>() where T : IService
    {
        return _services.TryGetValue(typeof(T), out var service) ? (T)service : default;
    }
}

public interface IService
{

}