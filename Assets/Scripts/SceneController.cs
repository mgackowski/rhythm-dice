using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    static SceneController instance;

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
        //FadeOut(3f);
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(Fade(duration, 1f));
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(duration, 0f));
    }

    private IEnumerator Fade(float duration, float targetAlpha)
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
        float alpha = newColour.a;

        while (elapsedTime <= duration)
        {
            newColour.a = Mathf.Lerp(alpha, targetAlpha, elapsedTime / duration);
            fadeScreen.color = newColour;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

    }

}
