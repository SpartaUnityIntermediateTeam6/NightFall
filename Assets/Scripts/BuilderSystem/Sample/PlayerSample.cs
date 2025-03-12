using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSample : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float moveSpeed = 5f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var momentum = new Vector3(x, 0f, z) * moveSpeed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(transform.position + momentum);
    }
}
