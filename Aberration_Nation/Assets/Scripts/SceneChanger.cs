using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Channel Scene IDs")]
    public int[] channels;

    [Header("Menu Location")]
    public GameObject Menu;

    private bool paused = false;

    public void ChangeChannel(InputAction.CallbackContext context)
    {
        int value = 0;
        if (context.ReadValue<float>() > 0) { SceneManager.LoadScene(channels[channels.Length-1]); }
        else { SceneManager.LoadScene(channels[0]); }

        int cur_scene = SceneManager.GetActiveScene().buildIndex;

        int next_channel = System.Array.IndexOf(channels, cur_scene);

        next_channel += value;

        //have the next channel value wrap around
        if (next_channel <= -1) { next_channel = channels.Length; }
        else if (next_channel >= channels.Length) { next_channel = 0; }
    }

    public void OpenOrCloseMenu()
    {
        print("Here");
        if (paused == false)
        {
            paused = true;

            //stop time
            Time.timeScale = 0f;

            Menu.SetActive(true);
        }
        else
        {
            paused = false;

            //restart time
            Time.timeScale = 1f;

            Menu.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(4);
        }
    }

}
