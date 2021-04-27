using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text playerHealthText;

    public Text doubleCountdownText;
    public GameObject doubleCountdownDisplay;
    public GameObject collectionDisplay;

    public Sprite missingPieceIcon;
    public Sprite collectedPieceIcon;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        GameSession.instance.healed.AddListener(() =>
        {
            PlayRestoreAnimation();
        });

        GameSession.instance.tookDamage.AddListener(() =>
        {
            PlayDamageAnimation();
        });

        GameSession.instance.collection.pieceCollectedEvent.AddListener((position) =>
        {
            PlayEmphasizeCheckboxAnimation(position);
        });

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

    public void PlayDamageAnimation()
    {
        animator.SetTrigger("TakeDamage");
    }

    public void PlayRestoreAnimation()
    {
        animator.SetTrigger("RestoreHealth");
    }

    public void PlayEmphasizeCheckboxAnimation(int position)
    {
        Image[] images = collectionDisplay.GetComponentsInChildren<Image>();
        images[position].GetComponent<Animator>().SetTrigger("Emphasize");
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
