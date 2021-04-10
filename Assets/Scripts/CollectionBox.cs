using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionBox : MonoBehaviour
{

    public GameObject gamePieceCollectionAnchor;
    private Animator animator;
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        textMesh = GetComponentInChildren<TextMesh>();

        UpdatePlayerName();
        UpdateOwnedPieceDisplay();
        animator.SetTrigger("OpenBox");
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

        //Transform[] anchors = GameObject.FindGameObjectsWithTag("GamePieceDisplay")
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Debug.Log("Loop " + (i * 6) + j);
                bool owned = GameSession.instance.collection.IsPieceOwned(i + 1, j + 1);
                if (owned)
                {
                    Debug.Log("YES");
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

}
