using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that you want to rotate.   " +
        "2. Hold down the left mouse button.  " +
        "3. Swipe left to go left.   " +
        "4. Swipe right to go right .   " +
        "5. Before swipping, make sure that you start the swipe from the left or the right part of the screen respectible and go over the center of it.   " +
        "6. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    #region Variables

    public Transform target;
    public float rotationSpeed = 5f;

    private float[] allowedAngles = { -120f, 0f, 120f };
    private float currentAngle = 0f;
    private float targetAngle = 0f;

    private bool isDragging = false;
    private bool wasOnTheLeft;

    public PlanetOptimization planet;
    #endregion

    void Start()
    {
        //target = GetComponent<Transform>();

        currentAngle = NormalizeAngle(target.localEulerAngles.z); //puts the euler angle range between -180 and 180
        targetAngle = currentAngle;
    }

    void Update()
    {
        ///Chooses which code to run, depanding on the build
        #if UNITY_EDITOR || UNITY_STANDALONE //runs before the game begin
            HandleMouseInput();
        #elif UNITY_IOS || UNITY_ANDROID
            HandleTouchInput();
        #endif

        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        target.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    #region TypesOfInteraction
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            wasOnTheLeft = Input.mousePosition.x < Screen.width / 2f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            bool isOnTheLeft = Input.mousePosition.x < Screen.width / 2f;

            if (isOnTheLeft != wasOnTheLeft)
            {
                wasOnTheLeft = isOnTheLeft;
                float direction = isOnTheLeft ? 1f : -1f;
                Rotation(direction);
            }
        }
    }


    void HandleTouchInput()
    {
        if (Input.touchCount > 0) //cheks if there is a finger on the screen
        {
            Touch touch = Input.GetTouch(0); 

            switch (touch.phase)
            {
                case TouchPhase.Began: //began touching
                    isDragging = true;
                    wasOnTheLeft = touch.position.x < Screen.width / 2f;
                    break;

                ///Execurtes the same code in both scenarios
                case TouchPhase.Moved: //moving across the screen
                case TouchPhase.Stationary: //the finger is held still
                    bool isOnTheLeft = touch.position.x < Screen.width / 2f;

                    if (isOnTheLeft != wasOnTheLeft)
                    {
                        float direction = isOnTheLeft ? -1f : 1f;
                        Rotation(direction);
                        wasOnTheLeft = isOnTheLeft;
                    }
                    break;

                ///Execurtes the same code in both scenarios
                case TouchPhase.Ended: //finger is lifted
                case TouchPhase.Canceled: //the touching is interruped
                    isDragging = false;
                    break;
            }
        }
    }

    #endregion

    void Rotation(float direction)
    {
        //float nextAngle = targetAngle - direction * 90f;
        float nextAngle = targetAngle + direction * 120f;

        nextAngle = Mathf.Clamp(nextAngle, -120f, 120f); //clamps the angle to 0, 90, -90

        if (System.Array.Exists(allowedAngles, angle => Mathf.Approximately(angle, nextAngle))) //if the "nextAngles" is almost equal to any of the values in "allowedAngles", then it updates the targetAngle
        {
            targetAngle = nextAngle;
        }
        StartCoroutine(planet.ShaderActivation(nextAngle));
    }
    
    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return Mathf.Round(angle / 120f) * 120f; //snaps to closest 90�
    }
}
