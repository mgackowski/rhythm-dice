using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    public GameObject labelPrefab;
    public GameObject playerLabelPrefab;
    public GameObject dieObject;
    public GameObject labelsParent;

    public Color enemyColour;
    public Color playerColour;

    private Die die;
    private Dictionary<GameObject, Label> labels; // K: tracked object, V: label component
    public List<Label> activeLabels;

    private GameObject dieLabelObject;
    private PlayerLabel dieLabel;
    //private bool dieLabelEmphasized = false;

    // Start is called before the first frame update
    void Start()
    {
        die = dieObject.GetComponent<Die>();

        GameObject playerLabelObject = Instantiate(playerLabelPrefab, labelsParent.transform, true);
        dieLabel = playerLabelObject.GetComponent<PlayerLabel>();
        dieLabel.trackedObject = die.transform;
        dieLabel.labelColour = playerColour;
        dieLabel.Show();

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
            label.playerLabel = dieLabel;
            newLabel.SetActive(false);
            labels.Add(enemy, label);
        }

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

        //if(GameSession.instance.doublePowerup && !dieLabel.emphasized)
        //{
        //    dieLabel.gameObject.SetActive(true);
        //    dieLabel.emphasized = true;
        //    dieLabel.Emphasize();
        //}
        //else if (!GameSession.instance.doublePowerup)
        //{
        //    dieLabel.Hide();
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Refactor
            GameObject label = labels[other.gameObject].gameObject;
            label.SetActive(true);
            Label labelComponent = label.GetComponent<Label>();
            activeLabels.Add(labelComponent);
            label.transform.position = other.gameObject.transform.position;
            labelComponent.Show();
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