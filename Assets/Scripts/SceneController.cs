using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            Debug.Log("Scene Controller initialised");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("Duplicate Scene Controller created; destroying " + gameObject.name);
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        //StartCoroutine(ChangeScene("TestScene",true,2f));
    }

    public IEnumerator ChangeScene(string nextScene, bool fade = true, float transitionDuration = 0.5f)
    {
        if(fade)
        {
            FadeOut(transitionDuration / 2);
            yield return StartCoroutine(Fade((transitionDuration / 2), 0f, 1f));
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        if(fade)
        {
            while(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != nextScene)
            {
                yield return null;
            }
            yield return StartCoroutine(Fade((transitionDuration / 2), 1f, 0f));
        }
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(duration, 0f, 1f));
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(duration, 1f, 0f));
    }

    private IEnumerator Fade(float duration, float firstAlpha, float targetAlpha)
    {
        GameObject fadeScreenObject = GameObject.FindGameObjectWithTag("FadeScreen");
        if (!fadeScreenObject)
        {
            Debug.LogError("The scene does not have a fade out image assigned.");
            yield break;
        }

        Image fadeScreen = fadeScreenObject.GetComponent<Image>();

        float elapsedTime = 0f;
        Color newColour = fadeScreen.color;
        newColour.a = firstAlpha;
        fadeScreen.color = newColour;
        float alpha = firstAlpha;
        while (elapsedTime <= duration)
        {
            newColour.a = Mathf.Lerp(alpha, targetAlpha, elapsedTime / duration);
            fadeScreen.color = newColour;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

    }

}
