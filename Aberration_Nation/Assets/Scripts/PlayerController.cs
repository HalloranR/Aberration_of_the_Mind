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
    public float jumpHeight = 5;
    public float gravity;

    [Header("Look Sensetivity")]
    public float xSensetivity = 90;
    public float ySensetivity = 90f;
    public float clamp = 85f;

    [Header("Transforms")]
    public Transform camTransform;

    [Header("Player Controller")]
    public InputAction playerControls;

    [Header("Channel Scene IDs")]
    public int[] channels;

    [Header("Menu Location")]
    public GameObject Menu;

    [Header("Shooter Mechanics")]
    public GameObject bullet_prefab;
    public GameObject gun;
    public float fireRate = 2f;
    public float bulletSpeed = 10f;

    [Header("Ground Detection")]
    public LayerMask groundMask;
    public float sphereRadius;

    [Header("Static Player Vars")]
    static Vector3 location = new Vector3(100f, 100f, 100f);
    static Vector3 rotation = Vector3.zero;
    static Vector3 velocity = Vector3.zero;

    private Rigidbody rb;
    private Vector2 currentMovement;
    private float currentJump = 0;
    private bool paused = false;
    private bool grounded = true;
    private float changeX;
    private float changeY;
    private float readyTime = 0;


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
        rb.velocity = velocity;
        //rb.rotation = rotation;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            gun.SetActive(true);
        }
        else
        {
            gun.SetActive(false);
        }
        rb.position = location;
    }

    void Start()
    {
        //set the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (readyTime != 0)
        {
            readyTime -= Time.deltaTime;
        }

        grounded = Physics.CheckSphere(rb.position, sphereRadius, groundMask);

        //Debug.Log("Position: " + rb.position.ToString());
        //Debug.Log("Velocity: " + rb.velocity.ToString());


        float y_velocity = rb.velocity.y;

        //Debug.Log(y_velocity);
        if (grounded)
        {
            y_velocity = currentJump;
        }
        else
        {
            y_velocity += -gravity * Time.deltaTime;
            currentJump = 0f;
        }

        //move the player
        rb.velocity = transform.right * currentMovement.x * moveSpeed + transform.up * y_velocity + transform.forward *  currentMovement.y * moveSpeed;

        //rotate the camera only around the 
        rb.transform.Rotate(Vector3.up * xSensetivity * changeX * Time.deltaTime);

        float xRotation = -changeY * Time.deltaTime * ySensetivity;
        xRotation = Mathf.Clamp(xRotation, -clamp, clamp);
        //Debug.Log(xRotation);
        Vector3 newRotation = rb.transform.eulerAngles;
        newRotation.x = xRotation;
        //Debug.Log(newRotation);
        //camTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //camTransform.eulerAngles = newRotation;
    }







    public void Jump()
    {
        if (grounded)
        {
            currentJump = jumpHeight;
        }
    }


    public void FireGun()
    {
        if (readyTime <= 0)
        {
            readyTime = fireRate;

            GameObject bullet = Instantiate(bullet_prefab, transform.position + transform.forward * 2, Quaternion.identity);

            //bullet.transform.Rotate(90, 0, 0);

            bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        }
    }
    

    public void Sprint()
    {

    }








    /// <summary>
    /// Set the rotation value for the player
    /// </summary>
    public void RotateX(InputAction.CallbackContext context)
    {
        if(paused == false)
        {
            changeX = context.ReadValue<float>();
        }
    }

    /// <summary>
    /// Set the rotation value for the player
    /// </summary>
    public void RotateY(InputAction.CallbackContext context)
    {
        if (paused == false)
        {
            if (context.ReadValue<float>() != 0)
            {
                changeY = context.ReadValue<float>();
                Debug.Log(changeY);
            }
        }
    }

    /// <summary>
    /// Set the movement value for the player
    /// </summary>
    public void MovePlayer(InputAction.CallbackContext context)
    {
        if(paused == false)
        {
            currentMovement = context.ReadValue<Vector2>();
            Debug.Log(currentMovement.ToString());
        }
    }

    /// <summary>
    /// When player presses one of either q or e swap the channel to the previous or next respetively
    /// </summary>
    /// <param name="context"></param>
    public void ChangeChannel(InputAction.CallbackContext context)
    {
        if (paused == false)
        {
            location = rb.position;

            int value = 0;
            if (context.ReadValue<float>() > 0) { value = 1; }
            else { value = -1; }

            int cur_scene = SceneManager.GetActiveScene().buildIndex;

            int next_channel = System.Array.IndexOf(channels, cur_scene);

            next_channel += value;

            //have the next channel value wrap around
            if (next_channel <= -1) { next_channel = channels.Length - 1; }
            else if (next_channel >= channels.Length) { next_channel = 0; }

            SceneManager.LoadScene(channels[next_channel]);
        }
    }


    public void DoAction(InputAction.CallbackContext context)
    {
        Debug.Log("JUmp");
        if(paused == false)
        {
            if (SceneManager.GetActiveScene().buildIndex == 2) { FireGun();  }
            else if (SceneManager.GetActiveScene().buildIndex == 3) { Jump(); }
            else if (SceneManager.GetActiveScene().buildIndex == 4) { Sprint();  }
        }
    }

    /// <summary>
    /// Toggles the main menu
    /// </summary>
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
