using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    public float countdown_timer;

    public float delay_timer = 5;

    public float scroll_rate;

    // Update is called once per frame
    void Update()
    {
        if (delay_timer > 0)
        {
            delay_timer -= Time.deltaTime;
        }
        else
        {
            countdown_timer -= Time.deltaTime;

            if (countdown_timer < 0)
            {
                Application.Quit();
                Debug.Log("Done");
            }
            else
            {
                this.transform.position = transform.position + transform.up * scroll_rate * Time.deltaTime;
            }
        }
    }
}
