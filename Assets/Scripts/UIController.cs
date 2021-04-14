using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text playerHealthText;

    public Text doubleCountdownText;
    public GameObject doubleCountdownDisplay;
    public GameObject collectionDisplay;

    public Sprite missingPieceIcon;
    public Sprite collectedPieceIcon;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        SetPlayerHealthText(GameSession.instance.health);
        UpdateCollectionDisplay();
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
        timerValue++;
        doubleCountdownText.text = timerValue.ToString();
    }

    //TODO: Optimise
    public void UpdateCollectionDisplay()
    {
        Image[] images = collectionDisplay.GetComponentsInChildren<Image>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                bool collected = GameSession.instance.collection.IsPieceCollectedOrOwned(i + 1, j + 1);
                if (collected)
                {
                    images[(i*6)+j].sprite = collectedPieceIcon;
                }
                else
                {
                    images[(i * 6) + j].sprite = missingPieceIcon;
                }
            }
        }
    }

}
