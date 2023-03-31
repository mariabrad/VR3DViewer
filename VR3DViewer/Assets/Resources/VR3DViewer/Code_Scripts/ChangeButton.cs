using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeButton : MonoBehaviour
{
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            Debug.Log("Now in next scene.");
        }
    }

    }
