using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private string currentLevel;

    private void Start()
    {
        currentLevel = SceneManager.GetActiveScene().name;
    }

    public void Reload()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

}
