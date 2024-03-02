using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerController : MonoBehaviour
{
    [Header("Movement vars")]
    public float moveSpeed = 5;

    [Header("Mouse Sensitivity")]
    public float sensX = 90;
    public float sensY = 90;

    [Header("Transforms")]
    public Transform camTransform;

    private Rigidbody rb;

    [SerializeField] private Vector3 movementInput = Vector3.zero;
    [SerializeField] private Vector2 rotationInput = Vector2.zero;

    void Start()
    {
        //set the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //grab some vars
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    //Use fixed update for movement
    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Gets the input from the player controller
    /// </summary>
    public void GetInput()
    {
        //start with the movement
        movementInput = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward;

        //grab the rotation
        rotationInput.x -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        rotationInput.y += Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;

        //clamp the x
        rotationInput.x = Mathf.Clamp(rotationInput.x, - 90f, 90f);
    }

    /// <summary>
    /// This func gets called from update and moves the player lol
    /// </summary>
    private void MovePlayer()
    {
        //move the player
        rb.AddForce(movementInput * moveSpeed * 10f, ForceMode.Force);

        //rotate the camera
        camTransform.rotation = Quaternion.Euler(rotationInput.x, rotationInput.y, 0);

        //rotate the player
        transform.rotation = Quaternion.Euler(0, rotationInput.y, 0);
    }
}
