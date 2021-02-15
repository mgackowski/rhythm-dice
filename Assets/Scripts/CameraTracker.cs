using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraTracker : MonoBehaviour
{
    public Transform target;
    public float smoothTime;
    public float targetZPosition = -34.43f;
    public float relativeYPosition = -17.4f;

    private Vector3 interpolatedPosition;
    private Vector3 cameraVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            targetPosition.y += relativeYPosition;
            targetPosition.z = targetZPosition;
            interpolatedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, smoothTime);
            //interpolatedPosition.z = targetZPosition;
            
            transform.position = interpolatedPosition;
        }
    }
}
