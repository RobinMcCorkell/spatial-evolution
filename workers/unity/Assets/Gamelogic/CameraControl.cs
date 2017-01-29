using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float panSpeed = 4.0f;
    public float zoomSpeed = 1.0f;
    private Vector3 mouseOrigin;
    private bool isPanning;
                                
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            mouseOrigin = Input.mousePosition;
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
            transform.Translate(move, Space.Self);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize += zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize -= zoomSpeed;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3, 6);

    }
}
