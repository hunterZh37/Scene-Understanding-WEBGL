using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    float dragSpeed = 15f; // Found this by trial and error - applied below in Update

    float scale;
    float normalizedScale = 250f; // Found this by trial and error - applied below in Update

    void Update()
    {
        Vector3 pos = transform.position;

        scale = Camera.main.orthographicSize;  // Use this to capture the zoom level

        if (Input.GetMouseButton(0))
        {
            pos.x -= Input.GetAxis("Mouse X") * dragSpeed * scale / normalizedScale;
            pos.z -= Input.GetAxis("Mouse Y") * dragSpeed * scale / normalizedScale;
        }

        transform.position = pos;
    }

}