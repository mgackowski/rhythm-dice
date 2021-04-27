using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameShop : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject newGameMenu;
    public GameObject levelCompleteMenu;
    public InputField playerName;
    public GameObject collectionBox;
    public GameObject boxObtainedCard;
    public GameObject quitButton;

    private AudioSource backgroundNoise;


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

        backgroundNoise = Camera.main.GetComponent<AudioSource>();
        StartCoroutine(FadeInAmbience(1f, 0.7f));

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            quitButton.SetActive(false);
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
        if (GameSession.instance.gameState == GameSession.State.MainMenu)
        {
            GameSession.instance.playerName = playerName.textComponent.text;
        }
        StartCoroutine(SceneController.instance.ChangeScene("Level_1",true,2f));
        StartCoroutine(FadeOutAmbience(1f));
        GameSession.instance.gameState = GameSession.State.InGameSession;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleBoxObtainedCard()
    {
        boxObtainedCard.SetActive(!boxObtainedCard.activeSelf);
    }

    public IEnumerator FadeOutAmbience(float fadeTime)
    {
        float startVolume = backgroundNoise.volume;

        while (backgroundNoise.volume > 0)
        {
            backgroundNoise.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    public IEnumerator FadeInAmbience(float fadeTime, float targetVolume)
    {
        while (backgroundNoise.volume < targetVolume)
        {
            backgroundNoise.volume += targetVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

}
