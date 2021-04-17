using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public enum State
    {
        MainMenu, InGameSession
    }

    public static GameSession instance;

    public State gameState = State.MainMenu;

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

    /* TODO: Extract Quality settings */
    public Resolution[] resolutions;
    public int[] qualityLevels;
    public bool[] postProcessOn;
    public int selectedResolutionIndex = 0;


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
        InitialiseResolutions();
    }

    private void LateUpdate()
    {
        //MaintainScreenResolution(); // TODO: Extract to specialised class
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleQualitySetting();
            ApplyQualitySetting();
        }
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
        if (doublePowerRemaining <= 0)
        {
            doublePowerup = false;
            doublePowerRemaining = maxDoubleDuration;
            doublePickupUsed.SetActive(true);
            doublePickupUsed = null;
        }
        doublePowerRemaining--;
    }

    public void SetUpLevel()
    {
        Heal();
        if (!tutorialCompleted) GameObject.FindGameObjectWithTag("Die").transform.position = Vector3.zero;
        collection.DiscardCollectedPieces();
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

    public void CompleteLevel()
    {
        collection.PersistCollectedPieces();
        GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>().interval = float.MaxValue;
        StartCoroutine(GetComponent<SceneController>().ChangeScene("GameShop", true, 1f, 1f));

    }

    public void LoadGame() {
        Debug.Log("Call to LoadGame()");
    }

    public void SaveGame() {
        Debug.Log("Call to SaveGame()");
    }

    /* Resolution and quality-handling methods - extract to specialised class */

    private void ApplyQualitySetting()
    {
        Screen.SetResolution(resolutions[selectedResolutionIndex].width, resolutions[selectedResolutionIndex].height, Screen.fullScreenMode);
        QualitySettings.SetQualityLevel(qualityLevels[selectedResolutionIndex]);
        GameObject postProcess = GameObject.FindGameObjectWithTag("PostProcessing");
        if (postProcess != null) postProcess.GetComponent<PostProcessVolume>().enabled = postProcessOn[selectedResolutionIndex];
        Canvas.ForceUpdateCanvases();
    }

    private void InitialiseResolutions()
    {
        resolutions = new Resolution[3];
        resolutions[0].width = 1920;
        resolutions[0].height = 1080;
        resolutions[0].refreshRate = Screen.currentResolution.refreshRate;
        resolutions[1].width = 1280;
        resolutions[1].height = 720;
        resolutions[1].refreshRate = Screen.currentResolution.refreshRate;
        resolutions[2].width = 960;
        resolutions[2].height = 540;
        resolutions[2].refreshRate = Screen.currentResolution.refreshRate;

        qualityLevels = new int[3] { 5, 3, 0};
        postProcessOn = new bool[3] { true, true, false };

    }

    private void ToggleQualitySetting()
    {
        selectedResolutionIndex = (selectedResolutionIndex + 1) % resolutions.Length;
    }

}