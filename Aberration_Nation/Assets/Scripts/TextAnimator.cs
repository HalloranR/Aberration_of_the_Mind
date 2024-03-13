using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextAnimator : MonoBehaviour
{
    [Header("Text Fields")]
    public TextMeshProUGUI quote;
    public TextMeshProUGUI author;
    public TextMeshProUGUI movie;

    [Header("Timer Variables")]
    public float fade_out_wait = 2f;
    public float change_scene_wait = 3f;


    [Header("Fade Variables")]
    public float fade_in_rate = 1f;
    public float fade_out_rate = 1f;

    [Header("Audio Components")]
    public AudioSource turn_on;


    void Start()
    {
        StartCoroutine(Timer());

        //quote.color = new Color(255, 255, 255, 0);
        //author.color = Color.white;
    }



    IEnumerator Timer()
    {
        Debug.Log("Started");

        yield return new WaitForSeconds(fade_out_wait);
        StartCoroutine(FadeOut(movie));
        StartCoroutine(FadeOut(author));
        StartCoroutine(FadeOut(quote));
        Debug.Log("Step5");

        turn_on.Play();

        yield return new WaitForSeconds(change_scene_wait);
        SceneManager.LoadScene(1);
        Debug.Log("Step6");
    }


    /// <summary>
    /// Fade in the text in the scene
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn(TextMeshProUGUI text)
    {
        Color c = text.color;
        for (float alpha = 1f; alpha <= 255; alpha += fade_in_rate)
        {
            c.a = alpha;
            text.color = c;
            yield return null;
        }
    }

    /// <summary>
    /// Fade out the text in the scene
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut(TextMeshProUGUI text)
    {
        Color c = text.color;

        for (float alpha = 1f; alpha >= 0; alpha -= fade_out_rate)
        {
            c.a = alpha;
            text.color = c;

            //use wait for seconds and not null so that it is conssistent across framerate
            yield return new WaitForSeconds(0.01f);
        }
    }
}
