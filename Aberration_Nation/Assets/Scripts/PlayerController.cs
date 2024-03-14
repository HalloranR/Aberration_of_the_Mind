using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Cursor = UnityEngine.Cursor;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movement vars")]
    public float moveSpeed = 5;
    public float sprintSpeed = 5;

    [Header("Rotation Speed")]
    public float rotationSpeed = 90;

    [Header("Transforms")]
    public Transform camTransform;

    [Header("Player Controller")]
    public InputAction playerControls;

    [Header("Channel Scene IDs")]
    public int[] channels;

    [Header("Menu Location")]
    public GameObject Menu;

    [Header("Player Location")]
    static Vector3 location = new Vector3(0f, 0f, 0f);

    private Rigidbody rb;
    private float currentMovement;
    private float currentRotation;  
    private bool paused = false;


    //enable and disable unity new input system
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Awake()
    {
        //grab some vars
        rb = GetComponent<Rigidbody>();

        rb.position = location;
    }

    void Start()
    {
        //set the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentRotation.ToString() + ", " + currentMovement.ToString());

        //move the player
        rb.velocity = transform.forward * moveSpeed * currentMovement;

        //rotate the player only around the y axis
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime * currentRotation, 0));

        print(rb.velocity.ToString() + ", " + rb.rotation.ToString());
    }







    public void Jump()
    {

    }


    public void FireGun()
    {

    }
    

    public void Sprint()
    {

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

    /// <summary>
    /// When player presses one of either q or e swap the channel to the previous or next respetively
    /// </summary>
    /// <param name="context"></param>
    public void ChangeChannel(InputAction.CallbackContext context)
    {
        location = rb.position;

        int value = 0;
        if(context.ReadValue<float>() > 0) { value = 1; }
        else { value = -1; }

        int cur_scene = SceneManager.GetActiveScene().buildIndex;

        int next_channel = System.Array.IndexOf(channels, cur_scene);

        next_channel += value;

        //have the next channel value wrap around
        if (next_channel <= -1) { next_channel = channels.Length-1; }
        else if(next_channel >= channels.Length) {  next_channel = 0; }

        SceneManager.LoadScene(channels[next_channel]);
    }


    public void LookAround(InputAction.CallbackContext context)
    {

    }

    public void DoAction(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().buildIndex == 2) { }
        else if(SceneManager.GetActiveScene().buildIndex == 3) { }
        else if(SceneManager.GetActiveScene().buildIndex == 4) { }
        else if(SceneManager.GetActiveScene().buildIndex == 5) { }
    }

    public void OpenOrCloseMenu()
    {
        if (paused == false)
        {
            paused = true;

            //stop time
            Time.timeScale = 0f;

            //turn on the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Menu.SetActive(true);
        }
        else
        {
            paused = false;

            //restart time
            Time.timeScale = 1f;

            //reset the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Menu.SetActive(false);
        }
    }
}
