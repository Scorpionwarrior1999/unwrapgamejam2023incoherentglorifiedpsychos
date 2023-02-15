using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restartfunction : MonoBehaviour
{
    [SerializeField]
    private string level;

    private void Start()
    {
        level = SceneManager.GetActiveScene().name;
    }

    public void Reload()
    {
        SceneManager.LoadScene(level);
    }
}
