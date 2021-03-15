using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public GameObject animationContainer;

    public void ResetPosition()
    {
        StartCoroutine(ResetPositionOnNextFrame());
    }

    IEnumerator ResetPositionOnNextFrame()
    {
        yield return new WaitForEndOfFrame();
        animationContainer.transform.localPosition = Vector3.zero;
    }

    public void DestroyAnimationContainerParent()
    {
        //Tut, tut, not the best practice to depend on the parent...
        Destroy(animationContainer.GetComponentInParent<Enemy>().gameObject);
    }
}
