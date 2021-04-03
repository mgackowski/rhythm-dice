using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    public GameObject labelPrefab;
    public GameObject dieObject;
    public GameObject labelsParent;

    private Die die;
    private Dictionary<GameObject, Label> labels; // K: tracked object, V: label component

    // Start is called before the first frame update
    void Start()
    {
        die = dieObject.GetComponent<Die>();

        labels = new Dictionary<GameObject, Label>();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            GameObject newLabel = Instantiate(labelPrefab, labelsParent.transform, true);
            Label label = newLabel.GetComponent<Label>();
            label.valueDisplayed = enemy.GetComponent<Enemy>().attackPower;
            label.trackedObject = enemy.transform;
            // TODO: set colour
            newLabel.SetActive(false);
            labels.Add(enemy, label);
        }

    }

    // Update is called once per frame
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

        //foreach (KeyValuePair<Transform, Label> entry in labels)
        //{
        //    
        //}


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Refactor
            GameObject label = labels[other.gameObject].gameObject;
            label.SetActive(true);
            label.transform.position = other.gameObject.transform.position;
            label.GetComponent<Label>().Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject label = labels[other.gameObject].gameObject;
            label.GetComponent<Label>().Hide();
            label.SetActive(false);     //TODO: Delay this
           
        }
    }

}