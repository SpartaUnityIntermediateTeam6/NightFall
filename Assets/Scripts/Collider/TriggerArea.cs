using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private readonly List<Rigidbody> _rigidbodies = new();

    public IReadOnlyList<Rigidbody> Rigidbodies => _rigidbodies;

    public event Action<Collider> OnTriggerEnterEvent = delegate { };
    public event Action<Collider> OnTriggerExitEvent = delegate { };

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            _rigidbodies.Add(other.attachedRigidbody);
        }

        OnTriggerEnterEvent?.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            _rigidbodies.Remove(other.attachedRigidbody);
        }

        OnTriggerExitEvent?.Invoke(other);
    }
}
