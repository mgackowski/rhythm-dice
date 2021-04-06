using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text playerHealthText;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        SetPlayerHealthText(GameSession.instance.health);
    }

    public void SetPlayerHealthText(int healthValue)
    {
        playerHealthText.text = healthValue.ToString();

    }

}
