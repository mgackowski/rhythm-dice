using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonTest()
    {
        Debug.Log("ButtonPressed");
    }

    public void StartLevel()
    {
        StartCoroutine(SceneController.instance.ChangeScene("Scene_0",true,2f));
    }
}
