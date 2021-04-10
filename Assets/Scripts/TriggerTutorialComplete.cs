using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Die"))
        {
            GameSession.instance.tutorialCompleted = true;
            gameObject.SetActive(false);
        }
    }
}
