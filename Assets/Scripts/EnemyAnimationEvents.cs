using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public GameObject animationContainer;

    public void ResetPosition()
    {
        animationContainer.transform.localPosition = Vector3.zero;
    }
}
