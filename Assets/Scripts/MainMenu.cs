using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame() { 
    
    
    }

    public void Settings() { 
    
    }

    public void Credits() { 
    
    }

    public void Exit() { 
        
    }
}
