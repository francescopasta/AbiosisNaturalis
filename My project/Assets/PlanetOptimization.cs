using System.Collections;
using UnityEngine;

public class PlanetOptimization : MonoBehaviour
{
    public WaterMeterUpdated waterMeter;

    public Transform planet;

    public Vector2 fireAreaEnabledRange;     // e.g., (25, 125)
    public Vector2 waterAreaEnabledRange;    // e.g., (335, 25) for -25 to 25
    public Vector2 crystalAreaEnabledRange;  // e.g., (235, 335) for -125 to -25

    public GameObject firePlain;
    public GameObject waterPlain;
    public GameObject crystalPlain;

    public GameObject fireDisplayIcon;
    public GameObject waterDisplayIcon;
    public GameObject crystalDisplayIcon;

    public GameObject fireLock;
    public GameObject crystalLock;

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
            fireDisplayIcon.SetActive(true);
            waterDisplayIcon.SetActive(false);
            crystalDisplayIcon.SetActive(false);
            fireLock.SetActive(!waterMeter.firePlantUnlocked);
            crystalLock.SetActive(false);
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
            fireDisplayIcon.SetActive(false);
            waterDisplayIcon.SetActive(true);
            crystalDisplayIcon.SetActive(false);
            fireLock.SetActive(false);
            crystalLock.SetActive(false);
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
            fireDisplayIcon.SetActive(false);
            waterDisplayIcon.SetActive(false);
            crystalDisplayIcon.SetActive(true);
            fireLock.SetActive(false);
            crystalLock.SetActive(!waterMeter.crystalPlantUnlocked);
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
