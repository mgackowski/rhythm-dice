using System.Collections;
using UnityEngine;

public class PlayerLabel : MonoBehaviour
{

    public Color labelColour;
    public int valueDisplayed;
    public float movementDuration = 0.1f;
    public Transform trackedObject;

    public MeshRenderer labelRenderer;
    public Animator animator;

    private TextMesh textMesh;

    private bool emphasized = false;

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
        Vector3 targetPosition = trackedObject.transform.position;

        targetPosition.z -= 2f; //TODO: Magic number
        float speed = 10f; // TODO: Magic number
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

        textMesh.text = valueDisplayed.ToString();

    }

    //TODO: Continue working here
    public void ReactToEnemyProximity(Vector3 positionDifference)
    {
        /*if(!emphasized)
        {
            gameObject.SetActive(true);
            emphasized = true;
            transform.position = trackedObject.transform.position;
            Emphasize();

            Vector3 targetPosition = trackedObject.position;
            targetPosition.z -= 2f;
            float angle = Mathf.Atan2(positionDifference.y, positionDifference.x) * 180 / Mathf.PI;
            if (angle >= 45 && angle < 135)
            { //up
                targetPosition.y -= 1;
                targetPosition.x -= 1;  // label: lower-left
            }
            else if (angle >= -45 && angle < 45)
            { //right
                targetPosition.y += 1;
                targetPosition.x -= 1;  // label: upper-left
            }
            else if (angle >= -135 && angle < -45)
            { //down
                targetPosition.y += 1;
                targetPosition.x += 1;  // label: upper-right
            }
            else
            { //left
                targetPosition.y -= 1;
                targetPosition.x += 1;  // label: lower-right
            }
            targetPosition.y -= 1; // correct for perspective;

        }*/
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
