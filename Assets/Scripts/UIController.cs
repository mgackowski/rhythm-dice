using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text playerHealthText;

    public Text doubleCountdownText;
    public GameObject doubleCountdownDisplay;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        SetPlayerHealthText(GameSession.instance.health);
        if (GameSession.instance.doublePowerup)
        {
            doubleCountdownDisplay.SetActive(true);
            SetDoubleTimerText(GameSession.instance.doublePowerRemaining);
        }
        else { doubleCountdownDisplay.SetActive(false); }
    }

    public void SetPlayerHealthText(int healthValue)
    {
        playerHealthText.text = healthValue.ToString();

    }

    public void SetDoubleTimerText(int timerValue)
    {
        doubleCountdownText.text = timerValue.ToString();
    }

}
