using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPressFeedback : MonoBehaviour
{
    public Text aKey;
    public Text sKey;
    public Text dKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            aKey.color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            sKey.color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            dKey.color = Color.blue;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            aKey.color = Color.white;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            sKey.color = Color.white;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            dKey.color = Color.white;
        }
    }
}
