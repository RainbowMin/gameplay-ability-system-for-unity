using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _input;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _input = new PlayerInput();
        _input.Enable();
        _input.Player.Move.performed += OnActivateMove;
        _input.Player.Move.canceled += OnDeactivateMove;
        _input.Player.Fire.performed += OnFire;
        _input.Player.Sweep.performed += OnSweep;
    }

    private void OnDestroy()
    {
        _input.Disable();
        _input.Player.Move.performed -= OnActivateMove;
        _input.Player.Move.canceled -= OnDeactivateMove;
        _input.Player.Fire.performed -= OnFire;
        _input.Player.Sweep.performed -= OnSweep;
    }

    private void Update()
    {
        // Player朝向始终面向鼠标
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var dir = (mousePos - transform.position);
        dir.z = 0;
        dir = dir.normalized;
        transform.up = dir;
    }

    public void Init()
    {
        
    }

    private void OnSweep(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnDeactivateMove(InputAction.CallbackContext context)
    {
        _rb.velocity = Vector2.zero;
    }

    private void OnActivateMove(InputAction.CallbackContext context)
    {
        var move = context.ReadValue<Vector2>();
        var velocity = _rb.velocity;
        velocity.x = move.x;
        velocity.y = move.y;
        velocity = velocity.normalized * 5;
        _rb.velocity = velocity;
    }

    private void OnDie()
    {
        GameRunner.Instance.GameOver();
        Destroy(gameObject);
    }
}
