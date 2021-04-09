using UnityEngine;
using UnityEngine.UI;

public class GameShop : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject newGameMenu;
    public InputField playerName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGameMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(true);
    }

    public void StartLevel()
    {
        GameSession.instance.playerName = playerName.textComponent.text;
        StartCoroutine(SceneController.instance.ChangeScene("Level_1",true,2f));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
