using UnityEngine;

public interface IInteractable<T>
{
    void Interaction(T vistor);
}
