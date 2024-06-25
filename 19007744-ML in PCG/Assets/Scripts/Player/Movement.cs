using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector2 movementSpeed = Vector2.one;
    public Rigidbody2D rigidbody;

    private InputScheme _inputScheme;
    private Vector2 _inputVector = Vector2.zero;

    private Inventory inventory;

    private void Awake()
    {
        InitialiseInput();
    }

    /// <summary>
    ///
    /// </summary>
    private void InitialiseInput()
    {
        _inputScheme = new InputScheme();
        _inputScheme._2DTopDown.Enable();
    }

    private void OnDestroy()
    {
        _inputScheme.Dispose();
    }

    // Update is called once per frame
    private void Update()
    {
        _inputVector = (_inputScheme._2DTopDown.Move.ReadValue<Vector2>()).normalized;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + (_inputVector * movementSpeed *Time.fixedDeltaTime));
    }
}
