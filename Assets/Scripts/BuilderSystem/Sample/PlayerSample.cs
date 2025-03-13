using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSample : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _moveSpeed = 5f;

    public float radius = 1f;
    public LayerMask targetLayers;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var momentum = new Vector3(x, 0f, z) * _moveSpeed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(transform.position + momentum);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Interaction();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Interaction()
    {
        //Sample
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayers, QueryTriggerInteraction.Collide);

        var target = colliders.Where(c => c.TryGetComponent(out IInteractable<PlayerSample> interatable)).
            OrderBy(c => c.ClosestPoint(transform.position)).
            FirstOrDefault();

        if (target != null)
        {
            target.GetComponent<IInteractable<PlayerSample>>().Interaction(this);
        }
    }
}
