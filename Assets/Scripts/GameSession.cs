using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession instance;

    private UIController uiController;
    private SceneController sceneController;

    public CollectionManager collection;

    public int defaultHealth = 10;
    public int maxDoubleDuration = 16;

    public int health = 10;
    public bool doublePowerup = false;
    public int doublePowerRemaining;
    private GameObject doublePickupUsed;

    public string playerName  = "Player";

    public int levelsCompleted = 0;
    public int polybagTokens = 0;

    public bool tutorialCompleted = false;
    public bool boxObtained = false;


    private void Awake()
    {
        if(!instance)
        {
            instance = this;
            Debug.Log("Game Session initialised");
            DontDestroyOnLoad(gameObject);
        } else
        {
            Debug.LogError("Duplicate Game Session created; destroying " + gameObject.name);
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += delegate { SetUpLevel(); };
    }

    public void ActivateDoublePowerup(GameObject pickupUsed)
    {
        doublePowerup = true;
        doublePowerRemaining = maxDoubleDuration;
        doublePickupUsed = pickupUsed;
        doublePickupUsed.SetActive(false);
    }

    public void DecreaseDoublePowerupTimer()
    {
        doublePowerRemaining--;
        if (doublePowerRemaining <= 0)
        {
            doublePowerup = false;
            doublePowerRemaining = maxDoubleDuration;
            doublePickupUsed.SetActive(true);
            doublePickupUsed = null;
        }
    }

    public void SetUpLevel()
    {
        Heal();
        if (!tutorialCompleted) GameObject.FindGameObjectWithTag("Die").transform.position = Vector3.zero;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(health - amount, 0);
        if(health <= 0)
        {
            GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>().interval = float.MaxValue;
            RestartLevel();
        }
    }

    public void Heal()
    {
        health = defaultHealth;
    }

    public void RestartLevel()
    {
        collection.DiscardCollectedPieces();
        StartCoroutine(GetComponent<SceneController>().ChangeScene(SceneManager.GetActiveScene().name, true, 1f, 1f));
    }

    public void LoadGame() {
        Debug.Log("Call to LoadGame()");
    }

    public void SaveGame() {
        Debug.Log("Call to SaveGame()");
    }


}