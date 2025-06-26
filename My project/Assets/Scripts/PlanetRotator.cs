using UnityEngine;
using System.Collections.Generic;

public class PlanetRotator : MonoBehaviour
{
    public List<Transform> areaRotations;  // Empty GameObjects representing desired positions
    public float rotationSpeed = 5f;
    public float swipeThreshold = 50f;

    private int currentIndex = 0;
    private Quaternion targetRotation;
    private Vector2 startTouchPos;
    private bool isDragging = false;

    void Start()
    {
        if (areaRotations.Count > 0)
            targetRotation = areaRotations[0].rotation;
    }

    void Update()
    {
        HandleInput();
        SmoothRotate();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector2 delta = startTouchPos - (Vector2)Input.mousePosition;
            ProcessSwipe(delta);
            isDragging = false;
        }

        // Mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                startTouchPos = touch.position;
            else if (touch.phase == TouchPhase.Ended)
                ProcessSwipe(startTouchPos - touch.position);
        }
    }

    void ProcessSwipe(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > swipeThreshold)
        {
            if (delta.x > 0)
                currentIndex = (currentIndex - 1 + areaRotations.Count) % areaRotations.Count;
            else
                currentIndex = (currentIndex + 1) % areaRotations.Count;

            targetRotation = areaRotations[currentIndex].rotation;
        }
    }

    void SmoothRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
