using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    public GameObject labelPrefab;
    public GameObject dieObject;

    private Die die;

    // Start is called before the first frame update
    void Start()
    {
        die = dieObject.GetComponent<Die>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject newLabel = Instantiate(labelPrefab, transform, true);
            newLabel.GetComponent<Label>().Show();
        }
    }

}
