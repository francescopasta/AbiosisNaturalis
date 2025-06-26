using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float moveDistance = 20f;
    public float moveSpeed = 5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    public void MoveCameraLeft()
    {
        if (!isMoving)
        {
            targetPosition = transform.position + Vector3.left * moveDistance;
            isMoving = true;
        }
    }

    public void MoveCameraRight()
    {
        if (!isMoving)
        {
            targetPosition = transform.position + Vector3.right * moveDistance;
            isMoving = true;
        }
    }
}
