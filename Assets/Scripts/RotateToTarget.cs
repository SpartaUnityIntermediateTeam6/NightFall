using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        if (target == null)
            return;

        transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
