using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform cameraTransform;
    private float shakeDuration = 1f, shakeAmount = 0.04f, decreaseFactor = 1.5f;

    private Vector3 originPosition;

    private void Start()
    {
        cameraTransform = GetComponent<Transform>();
        originPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            cameraTransform.localPosition = originPosition + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0;
            cameraTransform.localPosition = originPosition;
        }
    }

}
