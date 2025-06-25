using UnityEngine;
using UnityEngine.UI;

public class WaterMeter : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that will manage all the water meter.   " +
        "2. Drag the slider the will be used as the water meter.  " +
        "3. Make sure that the sliders max value is 100.   " +
        "4. Define the variables.  " +
        "5. For more information, hover your mouse over the variables.   " +
        "6. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    [Tooltip("This is for keeping track of the water and changing the slider value")]
    public float water;
    [Tooltip("This is the amount of water that will drain per a unit of time (timerEnd) will drain")]
    public float waterDrain;
    [Tooltip("This is the amount of water that will be added to the meter after the player clicks")]
    public float waterGainDefault;
    [Tooltip("This variable is used for updating the _waterGainDefault_. This will activate when the player is filling up the water meter")]
    public float waterGainUpdate;

    float timerStart;
    [Tooltip("This is the rate at which the water will drain. Closer to 0, will drain faster")]
    public float timerEnd = 3;

    [SerializeField] Slider waterSlider;

    void Start()
    {
        if (waterSlider == null)
        { 
            Debug.LogError("There is no water meter");
            return;
        }
    }

    void Update()
    {
        timerStart += Time.deltaTime;

        if (timerStart > timerEnd)
        {
            WaterDrain();
            timerStart -= timerEnd;
        }
    }

    private void WaterDrain()
    {
        if (water > 0)
        {
            water -= waterDrain;
            water = Mathf.Clamp(water, 0, 100);
            waterSlider.value = water;
        }
    }

    public void WaterGain()
    {
        water += waterGainDefault;
    }
}
