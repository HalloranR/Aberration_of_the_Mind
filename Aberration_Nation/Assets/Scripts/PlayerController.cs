using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement vars")]
    public float moveSpeed = 5;

    [Header("Rotation Speed")]
    public float rotationSpeed = 90;

    [Header("Transforms")]
    public Transform camTransform;

    [Header("Player Controller")]
    public InputAction playerControls;

    private Rigidbody rb;
    private float currentMovement;
    private float currentRotation;  


    //enable and disable unity new input system
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        //set the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //grab some vars
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //move the player
        rb.velocity = transform.forward * moveSpeed * currentMovement;

        //rotate the player only around the y axis
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime * currentRotation, 0));
    }


    /// <summary>
    /// Set the rotation value for the player
    /// </summary>
    public void RotatePlayer(InputAction.CallbackContext context)
    {
        currentRotation = context.ReadValue<float>();
    }

    /// <summary>
    /// Set the movement value for the player
    /// </summary>
    public void MovePlayer(InputAction.CallbackContext context)
    {
        currentMovement = context.ReadValue<float>();
    }
}
