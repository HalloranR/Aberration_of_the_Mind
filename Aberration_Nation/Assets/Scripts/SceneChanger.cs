using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int scene_num;
    public void ChangeScene()
    {
        SceneManager.LoadScene(scene_num);
    }
}
