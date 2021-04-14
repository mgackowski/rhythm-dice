using UnityEngine;
using UnityEngine.UI;

public class GameShop : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject newGameMenu;
    public GameObject levelCompleteMenu;
    public InputField playerName;
    public GameObject collectionBox;


    void Start()
    {
        if (GameSession.instance.gameState == GameSession.State.MainMenu)
        {
            mainMenu.SetActive(true);
        }
        else if (GameSession.instance.gameState == GameSession.State.InGameSession)
        {
            LevelCompleteMenu();
        }

    }

    public void LevelCompleteMenu()
    {
        mainMenu.SetActive(false);
        collectionBox.SetActive(true);
        levelCompleteMenu.SetActive(true);
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
        GameSession.instance.gameState = GameSession.State.InGameSession;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
