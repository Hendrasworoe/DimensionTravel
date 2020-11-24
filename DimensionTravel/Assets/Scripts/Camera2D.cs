using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public Transform area;

    Vector3 currentPosition;
    Vector3 currentScale;

    public void Set()
    {
        float height = area.localScale.y * 100;
        float width = area.localScale.x * 100;

        float h = Screen.height / height;
        float w = Screen.width / width;

        float ratio = w / h;
        float size = (height / 2) / 100f;

        if (w<h)
        {
            size /= ratio;
        }

        Camera.main.orthographicSize = size;

        Vector2 position = area.transform.position;

        Vector3 camPosition = position;
        Vector3 point = Camera.main.WorldToViewportPoint(camPosition);
        Vector3 delta = camPosition - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        Vector3 destination = transform.position + delta;

        transform.position = destination;
    }

    public void LateUpdate()
    {
        if (currentPosition != area.transform.position || currentScale != area.transform.localScale)
        {
            currentPosition = area.transform.position;
            currentScale = area.transform.localScale;
            Set();
        }
    }
}
