using UnityEngine;
using UnityEngine.UI;

public class WaterMeterUpdated : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that will manage all the water meter.   " +
        "2. Drag the slider the will be used as the water meter.  " +
        "3. Make sure that the sliders max value is 100.   " +
        "4. Define the variables.  " +
        "5. For more information, hover your mouse over the variables.   " +
        "6. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    [Tooltip("This is for keeping track of the water and changing the slider value")] //fix it 
    public float[] waterLevels = new float[3];
    [Tooltip("This is the amount of water that will drain per a unit of time (timerEnd) will drain")]
    public float waterDrain;
    [Tooltip("This is the amount of water that will be added to the meter after the player clicks")]
    public float[] waterGainDefault = new float[3];
    [Tooltip("This variable is used for updating the _waterGainDefault_. This will activate when the player is filling up the water meter")]
    public float waterGainUpdate;

    float timerStart;
    [Tooltip("This is the rate at which the water will drain. Closer to 0, will drain faster")]
    public float timerEnd = 3;

    [SerializeField] Slider[] waterSliders = new Slider[3]; // Assign sliders via inspector for each flower

    void Start()
    {
        if (waterSliders == null)
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
            for (int i = 0; i < waterLevels.Length; i++)
            {
                Drain(i);
            }

            timerStart -= timerEnd;
        }
    }

    public void Drain(int flowerIndex)
    {
        if (flowerIndex < 0 || flowerIndex >= waterLevels.Length) return;

        if (waterLevels[flowerIndex] > 0)
        {
            waterLevels[flowerIndex] -= waterDrain;
            waterLevels[flowerIndex] = Mathf.Clamp(waterLevels[flowerIndex], 0, 100);

            if (waterSliders[flowerIndex] != null)
                waterSliders[flowerIndex].value = waterLevels[flowerIndex];

            if (waterLevels[flowerIndex] == 0 && waterGainDefault[flowerIndex] == 0)
            {
                waterGainDefault[flowerIndex] = waterGainUpdate;
            }
        }
    }

    public void Gain(int flowerIndex)
    {
        if(flowerIndex < 0 || flowerIndex >= waterLevels.Length) return;

        float gainAmount = (waterLevels[flowerIndex] == 0) ? waterGainUpdate : waterGainDefault[flowerIndex];

        waterLevels[flowerIndex] += gainAmount;
        waterLevels[flowerIndex] = Mathf.Clamp(waterLevels[flowerIndex], 0, 100);

        if (waterSliders[flowerIndex] != null)
            waterSliders[flowerIndex].value = waterLevels[flowerIndex];

        // Disable further gain if flower is full
        if (waterLevels[flowerIndex] >= 100)
        {
            waterGainDefault[flowerIndex] = 0;
        }
    }

    /* void Start()
     {
         for (int i = 0; i < waterSliders.Length; i++)
         {
             if (waterSliders[i] == null)
                 Debug.LogError($"Missing water slider for flower {i}");
         }
     }

     void Update()
     {
         timerStart += Time.deltaTime;
         if (timerStart > timerEnd)
         {
             DrainAllFlowers();
             timerStart -= timerEnd;
         }
     }

     private void DrainAllFlowers()
     {
         for (int i = 0; i < waterLevels.Length; i++)
         {
             if (waterLevels[i] > 0)
             {
                 waterLevels[i] -= waterDrain;
                 waterLevels[i] = Mathf.Clamp(waterLevels[i], 0, 100);
                 if (waterSliders[i] != null)
                     waterSliders[i].value = waterLevels[i];
             }
         }
     }

     public void GainWater(int flowerIndex)
     {
         if (flowerIndex < 0 || flowerIndex >= waterLevels.Length) return;

         // Optional: auto-adjust gain amount
         float gainAmount = (waterLevels[flowerIndex] == 0) ? waterGainUpdate : waterGainDefault;

         waterLevels[flowerIndex] += gainAmount;
         waterLevels[flowerIndex] = Mathf.Clamp(waterLevels[flowerIndex], 0, 100);

         if (waterSliders[flowerIndex] != null)
             waterSliders[flowerIndex].value = waterLevels[flowerIndex];
     }

     public float GetWaterLevel(int flowerIndex)
     {
         if (flowerIndex < 0 || flowerIndex >= waterLevels.Length) return 0;
         return waterLevels[flowerIndex];
     }*/
}