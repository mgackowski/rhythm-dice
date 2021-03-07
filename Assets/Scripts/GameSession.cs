using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession instance;

    public string playerName  = "Player";
    // store current dice collection
    // store current game piece collection
    public int levelsCompleted = 0;
    public int polybagTokens = 0;
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

    public void LoadGame() {
        Debug.Log("Call to LoadGame()");
    }

    public void SaveGame() {
        Debug.Log("Call to SaveGame()");
    }


}