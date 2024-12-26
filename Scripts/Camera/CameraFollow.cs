using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // הדמות שאחריה המצלמה עוקבת
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //make sure the camera doesn't move in the z axis
        smoothedPosition.z = transform.position.z; 
        transform.position = smoothedPosition;
    }
}
