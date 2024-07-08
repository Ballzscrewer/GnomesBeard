using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(SaveManager.instance.LoadGame());
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void StartGameAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartGameAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}


