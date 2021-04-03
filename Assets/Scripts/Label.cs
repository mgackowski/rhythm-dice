using System.Collections;
using UnityEngine;

public class Label : MonoBehaviour
{

    public Color labelColour;
    public int valueDisplayed;
    public float movementDuration = 0.1f;

    public MeshRenderer labelRenderer;
    public Animator animator;

    private TextMesh textMesh;

    void Start()
    {
        Material material = labelRenderer.material;
        material.color = labelColour;
        material.SetColor("_EmissionColor", labelColour);

        textMesh = gameObject.GetComponentInChildren<TextMesh>();
        textMesh.text = valueDisplayed.ToString();
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Emphasize()
    {
        animator.SetTrigger("Emphasize");
    }

    public void MoveTo(Vector3 newPosition)
    {
        newPosition.z = transform.position.z; // ignore changes to height
        StartCoroutine(_Move(newPosition));
    }

    private IEnumerator _Move(Vector3 newPosition)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < movementDuration)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, elapsedTime / movementDuration);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }

}
