using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIndicator : MonoBehaviour
{
    public float fadeOutDuration = 1f;
    private Material thisMaterial;

    // Start is called before the first frame update
    void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
        Color thisColour = thisMaterial.color;
        thisColour.a = 0.5f;
        thisMaterial.color = thisColour;

        StartCoroutine(FadeOutAndDestroy(fadeOutDuration));
    }

    IEnumerator FadeOutAndDestroy(float duration)
    {
        float elapsedTime = 0f;
        Color newColour = thisMaterial.color;
        float alpha = newColour.a;


        while (elapsedTime <= duration)
        {
            newColour.a = Mathf.Lerp(alpha, 0f, elapsedTime / duration);
            thisMaterial.color = newColour;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        Destroy(gameObject);

    }
}
