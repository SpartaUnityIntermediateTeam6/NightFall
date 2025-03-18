using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IVisitable
{
    //Reference
    private TPSCharacterController _controller;
    private PlayerStats _stats;
    private Inventory _inventory;
    private InputReader _inputReader;
    private Camera _camera;
    public Inventory Inventory => _inventory;

    public event Action OnInteractionEvent = delegate { };

    void Awake()
    {
        _controller = GetComponent<TPSCharacterController>();
        _stats = GetComponent<PlayerStats>();
        _inputReader = GetComponent<InputReader>();
        //Sample Code
        _camera = Camera.main;
        _inventory = GetComponent<Inventory>();
        //Sample Code
        _camera = Camera.main;

        GameManager.Instance.player = this.transform;

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Sample Code
        if (_inputReader.Input.Player.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            Move(_inputReader.Input.Player.Move.ReadValue<Vector2>());
        }

        if (_inputReader.Input.Player.Jump.ReadValue<float>() != 0f)
        {
            Jump();
        }
    }

    public void Move(Vector2 vec)
    {
        var right = Vector3.ProjectOnPlane(_camera.transform.right, transform.up) * vec.x;
        var forward = Vector3.ProjectOnPlane(_camera.transform.forward, transform.up) * vec.y;

        var project = right + forward;

        _controller.Move(project * Time.deltaTime * _stats.MoveSpeed);
    }

    public void Jump()
    {
        _controller.Jump(_stats.JumpPower);
    }

    public void Interaction()
    {
        OnInteractionEvent?.Invoke();
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Cancel(IVisitor visitor)
    {
        visitor.Leave(this);
    }
}
