using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelAnimationEvents : MonoBehaviour
{
    public GameObject label;

    public void Deactivate()
    {
        if (label != null ) label.SetActive(false);
    }

}
