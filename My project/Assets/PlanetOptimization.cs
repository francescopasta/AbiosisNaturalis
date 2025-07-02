using System.Collections;
using UnityEngine;

public class PlanetOptimization : MonoBehaviour
{
    public Transform planet;

    public Vector2 fireAreaEnabledRange;     // e.g., (25, 125)
    public Vector2 waterAreaEnabledRange;    // e.g., (335, 25) for -25 to 25
    public Vector2 crystalAreaEnabledRange;  // e.g., (235, 335) for -125 to -25

    public GameObject firePlain;
    public GameObject waterPlain;
    public GameObject crystalPlain;

    private bool waterEnabled = false;
    private bool fireEnabled = false;
    private bool crystalEnabled = false;

    public float timer;
    void Update()
    {
       // float zRotation = planet.transform.eulerAngles.z;

        
    }


    public IEnumerator ShaderActivation(float zRotation) 
    {
        if (IsAngleInRange(zRotation, fireAreaEnabledRange.x, fireAreaEnabledRange.y) && !fireEnabled)
        {
            yield return new WaitForSeconds(timer);
            Debug.Log("Enabling fire");
            firePlain.SetActive(true);
            waterPlain.SetActive(false);
            crystalPlain.SetActive(false);
            fireEnabled = true;
            waterEnabled = false;
            crystalEnabled = false;
        }
        else if (IsAngleInRange(zRotation, waterAreaEnabledRange.x, waterAreaEnabledRange.y) && !waterEnabled)
        {
            yield return new WaitForSeconds(timer);
            Debug.Log("Enabling water");
            firePlain.SetActive(false);
            waterPlain.SetActive(true);
            crystalPlain.SetActive(false);
            fireEnabled = false;
            waterEnabled = true;
            crystalEnabled = false;
        }
        else if (IsAngleInRange(zRotation, crystalAreaEnabledRange.x, crystalAreaEnabledRange.y) && !crystalEnabled)
        {
            yield return new WaitForSeconds(timer);
            Debug.Log("Enabling crystal");
            firePlain.SetActive(false);
            waterPlain.SetActive(false);
            crystalPlain.SetActive(true);
            fireEnabled = false;
            waterEnabled = false;
            crystalEnabled = true;
        }
        
    }
    // Helper to check if angle is within a circular range
    bool IsAngleInRange(float angle, float min, float max)
    {
        angle = NormalizeAngle(angle);
        min = NormalizeAngle(min);
        max = NormalizeAngle(max);

        if (min < max)
            return angle >= min && angle <= max;
        else
            return angle >= min || angle <= max;
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
