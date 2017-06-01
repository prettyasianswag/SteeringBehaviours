using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float zoomSpeed = 5f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float minDistance = 5f;
    public float maxDistance = 20f;

    public float orthoSizeMin = 5f;
    public float orthoSizeMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    private float orthoSize = 5;

    // Use this for initialization
    void Start()
    {
        orthoSize = Camera.main.orthographicSize;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (Camera.main.orthographic)
        {
            orthoSize = Mathf.Clamp(orthoSize - scroll, orthoSizeMin, orthoSizeMax);
            Camera.main.orthographicSize = orthoSize;
        }
        else
        {
            distance = Mathf.Clamp(distance - scroll, minDistance, maxDistance);
        }

        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
