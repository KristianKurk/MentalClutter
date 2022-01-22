using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject thoughts;
    public Transform thoughtsParent;

    void Awake()
    {
        if(instance != null) return;

        instance = this;
    }
}
