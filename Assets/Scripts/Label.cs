using System.Collections;
using UnityEngine;

public class Label : MonoBehaviour
{

    public Color labelColour;
    public int valueDisplayed;
    public float movementDuration = 0.1f;
    public Transform trackedObject;
    public Transform otherObject;
    public PlayerLabel playerLabel;

    public MeshRenderer labelRenderer;
    public Animator animator;

    protected TextMesh textMesh;
    public  bool disabled = false;
    public bool emphasized = false;

    void Start()
    {
        Material material = labelRenderer.material;
        material.color = labelColour;
        material.SetColor("_EmissionColor", labelColour);

        textMesh = gameObject.GetComponentInChildren<TextMesh>();
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
        }

        textMesh.text = valueDisplayed.ToString();

        Vector3 objectPosition = trackedObject.position;
        Vector3 difference = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        if (otherObject != null)
        {
            Vector3 otherPosition = otherObject.position;
            difference = otherPosition - objectPosition;
        }

        if (difference.magnitude < 1.4f)
        {

            if (!emphasized)
            {
                Emphasize();
                emphasized = true;
                playerLabel.ReactToEnemyProximity(difference);
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

        objectPosition.z -= 2f; //TODO: Magic number
        float speed = 10f; // TODO: Magic number
        transform.position = Vector3.Lerp(transform.position, objectPosition, Time.deltaTime * speed);
        
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
