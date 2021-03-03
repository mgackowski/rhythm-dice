using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    public string playerName = "Player";
    // store current dice collection
    //s store current game piece collection
    public int levelsCompleted = 0;
    public int polybagTokens = 0;
    public bool boxObtained = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}