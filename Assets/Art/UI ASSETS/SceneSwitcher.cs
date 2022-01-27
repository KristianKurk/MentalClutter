using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public int SceneBuildIndex;
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneBuildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}