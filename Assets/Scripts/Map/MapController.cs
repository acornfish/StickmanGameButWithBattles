using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [Header("Zoom Ayarları")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 7f;  // zoom out sınırı düşürüldü

    [Header("Hareket Ayarları")]
    public float moveSpeed = 0.5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;
    private Vector3 lastMousePosition;

    public TextMeshProUGUI levelName;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleZoom();
        HandleMouseDrag();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(2)) // Orta mouse tuşuna basıldığında
        {
            lastMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 currentMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = lastMousePosition - currentMousePosition;

            transform.position += difference;
        }
    }

    void ClampCameraPosition()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = minBounds.x + camWidth;
        float maxX = maxBounds.x - camWidth;
        float minY = minBounds.y + camHeight;
        float maxY = maxBounds.y - camHeight;

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
        transform.position = clampedPos;
    }

    public void openLevel()
    {
        string levn = levelName.text;
        SceneManager.LoadScene(1);
        return;
    }
}
