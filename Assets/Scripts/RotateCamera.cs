using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float speedRotate;

    private void Update()
    {
        transform.Rotate(0f, speedRotate * Time.deltaTime, 0f);
    }
}
