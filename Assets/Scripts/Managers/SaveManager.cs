using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private string _worldSceneName;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadGame()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_worldSceneName);
        yield return null;
    }
}
