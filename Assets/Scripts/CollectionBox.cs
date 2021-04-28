using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectionBox : MonoBehaviour
{

    public GameObject gamePieceCollectionAnchor;
    private Animator animator;
    private TextMesh textMesh;

    public UnityEvent boxObtainedEvent;

    // Wire these dice for the 1-level prototype
    public GameObject[] displayDice;

    void Start()
    {
        animator = GetComponent<Animator>();
        textMesh = GetComponentInChildren<TextMesh>();

        UpdatePlayerName();
        UpdateOwnedPieceDisplay();

        if (!GameSession.instance.boxObtained)
        {
            StartCoroutine(PlayFirstCollectionScene());
        }
        else
        {
            StartCoroutine(PlaySubsequentTime());
        }
       
    }

    public void UpdatePlayerName()
    {
        textMesh.text = GameSession.instance.playerName;
    }

    public void UpdateOwnedPieceDisplay()
    {
        List<Transform> gamePieceDisplayAnchor = new List<Transform>();
        foreach (Transform child in gamePieceCollectionAnchor.transform)
        {
            gamePieceDisplayAnchor.Add(child);
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Debug.Log("Loop " + (i * 6) + j);
                bool owned = GameSession.instance.collection.IsPieceCollectedOrOwned(i + 1, j + 1);
                if (owned)
                {
                    GameObject prefab = GameSession.instance.collection.GetPrefab(i + 1, j + 1);
                    GameObject model = Instantiate(prefab, gamePieceDisplayAnchor[(i * 6) + j]);
                    model.transform.localPosition = Vector3.zero;
                    model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    model.transform.RotateAround(model.transform.position, Vector3.down, 90f);
                    model.GetComponent<Enemy>().enabled = false;
                    Rotate rotationScript = model.AddComponent<Rotate>();
                    rotationScript.xRotation = 45f;
                    //rotationScript.yRotation = 10f;
                    rotationScript.zRotation = -45f;
                }
            }
        }
    }

    public IEnumerator PlayFirstCollectionScene()
    {

        yield return new WaitForSeconds(1f);
        boxObtainedEvent.Invoke();
        GameSession.instance.boxObtained = true;
        yield return new WaitForSeconds(3f);
        animator.SetTrigger("OpenBox");
        boxObtainedEvent.Invoke();

        // Dice hardwired for prototype; not using die values from collection

        yield return new WaitForSeconds(1f);
        displayDice[1].SetActive(true);
        StartCoroutine(Shrink(displayDice[1]));

        if(!GameSession.instance.collection.IsSetFlaggedCollectedOnce(1) && GameSession.instance.collection.IsSetCollected(1))
        {
            displayDice[2].SetActive(true);
            StartCoroutine(Shrink(displayDice[2]));
            GameSession.instance.collection.SetFlagSetCollectedOnce(1, true);
        }

        if (!GameSession.instance.collection.IsSetFlaggedCollectedOnce(2) && GameSession.instance.collection.IsSetCollected(2))
        {
            displayDice[3].SetActive(true);
            StartCoroutine(Shrink(displayDice[3]));
            GameSession.instance.collection.SetFlagSetCollectedOnce(2, true);
        }

    }

    public IEnumerator PlaySubsequentTime()
    {
        displayDice[1].SetActive(true);
        if (GameSession.instance.collection.IsSetFlaggedCollectedOnce(1)) displayDice[2].SetActive(true);
        if (GameSession.instance.collection.IsSetFlaggedCollectedOnce(2)) displayDice[3].SetActive(true);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("OpenBox");
        yield return new WaitForSeconds(1f);

        if (!GameSession.instance.collection.IsSetFlaggedCollectedOnce(1) && GameSession.instance.collection.IsSetCollected(1))
        {
            displayDice[2].SetActive(true);
            StartCoroutine(Shrink(displayDice[2]));
            GameSession.instance.collection.SetFlagSetCollectedOnce(1, true);
        }

        if (!GameSession.instance.collection.IsSetFlaggedCollectedOnce(2) && GameSession.instance.collection.IsSetCollected(2))
        {
            displayDice[3].SetActive(true);
            StartCoroutine(Shrink(displayDice[3]));
            GameSession.instance.collection.SetFlagSetCollectedOnce(2, true);
        }
    }

    private IEnumerator Shrink(GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        obj.transform.localScale *= 2;
        while (obj.transform.localScale.magnitude > originalScale.magnitude)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, originalScale, Time.deltaTime);
            yield return null;
        }

    }

}
