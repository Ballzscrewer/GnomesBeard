using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
}

