using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    public GameObject labelPrefab;
    public GameObject dieObject;
    public GameObject labelsParent;

    public Color enemyColour;
    public Color playerColour;

    private Die die;
    private Dictionary<GameObject, Label> labels; // K: tracked object, V: label component
    public List<Label> activeLabels;

    private Label dieLabel;
    private GameObject dieLabelGameObject;

    // Start is called before the first frame update
    void Start()
    {
        die = dieObject.GetComponent<Die>();

        labels = new Dictionary<GameObject, Label>();
        activeLabels = new List<Label>();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            GameObject newLabel = Instantiate(labelPrefab, labelsParent.transform, true);
            Label label = newLabel.GetComponent<Label>();
            label.valueDisplayed = enemy.GetComponent<Enemy>().attackPower;
            label.trackedObject = enemy.transform;
            label.otherObject = die.transform;
            label.labelColour = enemyColour;
            newLabel.SetActive(false);
            labels.Add(enemy, label);
        }

        dieLabelGameObject = Instantiate(labelPrefab, labelsParent.transform, true);
        dieLabel = dieLabelGameObject.GetComponent<Label>();
        dieLabel.labelColour = playerColour;
        dieLabel.trackedObject = die.transform;
        dieLabel.otherObject = transform; // Temporary
        dieLabel.gameObject.SetActive(false);
        //dieLabel.Show();

    }

    void Update()
    {
        float colliderRotation = 0f;
        switch (die.movementDirection)
        {
            case Direction.Up: colliderRotation = 0f;
                break;
            case Direction.Right: colliderRotation = -90f;
                break;
            case Direction.Down: colliderRotation = 180f;
                break;
            case Direction.Left: colliderRotation = 90f;
                break;
            default:
                break;
        }

        transform.position = dieObject.transform.position;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, colliderRotation);


        dieLabel.valueDisplayed = die.currentAttack;


        bool emphasizeDieLabel = false;
        //TODO: Iterate over active labels; should be triggered by some event instead
        List<Label> destroyedLabels = new List<Label>(activeLabels);
        foreach (Label label in destroyedLabels)
        {
            if (!label.gameObject.activeSelf) activeLabels.Remove(label);
        }
        foreach (Label label in activeLabels)
        {
            if (label.emphasized && !label.disabled) emphasizeDieLabel = true;
            dieLabel.otherObject = label.trackedObject.transform;
        }
        if(emphasizeDieLabel)
        {
            dieLabel.gameObject.SetActive(true);
            //dieLabel.Show();   
        }
        else
        {
            dieLabel.otherObject = transform;
            //dieLabel.gameObject.SetActive(false); // Should be controlled by Hide animation trigger instead
            dieLabel.Hide();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Refactor
            GameObject label = labels[other.gameObject].gameObject;
            label.SetActive(true);
            activeLabels.Add(label.GetComponent<Label>());
            label.transform.position = other.gameObject.transform.position;
            label.GetComponent<Label>().Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Label label = labels[other.gameObject];
            label.Hide();
            activeLabels.Remove(label);
            
           
        }
    }

}