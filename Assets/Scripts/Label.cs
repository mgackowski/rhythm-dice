using System.Collections;
using UnityEngine;

public class Label : MonoBehaviour
{

    public Color labelColour;
    public int valueDisplayed;
    public float movementDuration = 0.1f;
    public Transform trackedObject;
    public Transform die;

    public MeshRenderer labelRenderer;
    public Animator animator;

    private TextMesh textMesh;
    private bool disabled = false;
    private bool emphasized = false;

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

    private void Update()
    {
        if (!disabled && (trackedObject.gameObject == null || !trackedObject.gameObject.activeSelf))
        {
            disabled = true;
            animator.SetTrigger("Hide");
            //gameObject.SetActive(false);
        }

        Vector3 objectPosition = trackedObject.position;
        Vector3 diePosition = die.position;
        Vector3 difference = diePosition - objectPosition;
        // TODO: examine the magnitude to determine if label should grow

        if (difference.magnitude < 1.4f)
        {
            if (!emphasized)
            {
                Emphasize();
                emphasized = true;
            }

            float angle = Mathf.Atan2(difference.y, difference.x) * 180 / Mathf.PI;
            if (angle >= 45 && angle < 135)
            { //up
                objectPosition.y -= 1;
                objectPosition.x -= 1;  // label: lower-left
            }
            else if (angle >= -45 && angle < 45)
            { //right
                objectPosition.y += 1;
                objectPosition.x -= 1;  // label: upper-left
            }
            else if (angle >= -135 && angle < -45)
            { //down
                objectPosition.y += 1;
                objectPosition.x += 1;  // label: upper-right
            }
            else
            { //left
                objectPosition.y -= 1;
                objectPosition.x += 1;  // label: lower-right
            }
            objectPosition.y -= 1; // correct for perspective;

        }
        else
        {
            Show();
            emphasized = false;
        }

        //Vector3 newPosition = trackedObject.position;
        objectPosition.z -= 2f; //TODO: Magic number
        float speed = 10f; // TODO: Magic number
        transform.position = Vector3.Lerp(transform.position, objectPosition, Time.deltaTime * speed);

        
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /*public void MoveTo(Vector3 newPosition)
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
    }*/

}
