using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;

    public const string MAIN_SCENE = "RhythmTest";  //needs to be changed to actual scene

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(StartNewGame);
        buttons[1].onClick.AddListener(Settings);
        buttons[2].onClick.AddListener(Credits);
        buttons[3].onClick.AddListener(Exit);
    }

    public void StartNewGame() {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void Settings() { 
    
    }

    public void Credits() { 
    
    }

    public void Exit() { 
        
    }
}
