using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession instance;

    //public GameObject levelUIprefab;

    private UIController uiController;
    private SceneController sceneController;

    public int defaultHealth = 10;
    public int health = 10;

    public string playerName  = "Player";
    // store current dice collection
    // store current game piece collection
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

    //private void Update()
    //{
    //    
    //}

    public void SetUpLevel()
    {
        health = defaultHealth;
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

    public void RestartLevel()
    {
        StartCoroutine(GetComponent<SceneController>().ChangeScene(SceneManager.GetActiveScene().name, true, 1f, 1f));
    }

    public void LoadGame() {
        Debug.Log("Call to LoadGame()");
    }

    public void SaveGame() {
        Debug.Log("Call to SaveGame()");
    }


}