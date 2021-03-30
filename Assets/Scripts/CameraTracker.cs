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

    public float defaultZoom = 13.9f;
    public float zoomAmount = 2f;
    public float zoomDuration = 1f;

    private Vector3 interpolatedPosition;
    private Vector3 cameraVelocity = Vector3.zero;
    private Camera thisCamera;

    private void Start()
    {
       thisCamera = GetComponent<Camera>();
    }

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

    public void ZoomIn() {
        StartCoroutine(ZoomCamera(defaultZoom - zoomAmount, zoomDuration));
    }
    public void ZoomBack()
    {
        StartCoroutine(ZoomCamera(defaultZoom, zoomDuration));
    }

    private IEnumerator ZoomCamera(float target, float duration)
    {
        float currentFOV = thisCamera.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            thisCamera.fieldOfView = Mathf.Lerp(currentFOV, target, elapsedTime / duration);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        
    }


}
