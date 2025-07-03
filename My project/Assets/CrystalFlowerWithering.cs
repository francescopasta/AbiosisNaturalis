using UnityEngine;

public class CrystalFlowerWithering : MonoBehaviour
{
    public WaterMeterUpdated waterMeter;
    public CrystalFlowerUpdated crystalFlower;
    public bool witheringCrystal;
    public float witherTimerCrystal;
    public float timeToWither;
    // Update is called once per frame
    void Update()
    {
        if (waterMeter.waterLevels[2] >= 100) 
        {
            witherTimerCrystal += Time.deltaTime;
            if (witherTimerCrystal >= timeToWither)
            {
                CrystalFlowerWither();
            }
        }
        if (waterMeter.waterLevels[2] < 100)
        {
            witheringCrystal = false;
            witherTimerCrystal = 0;
        }
    }
    public void CrystalFlowerWither() 
    {
        crystalFlower.ResetManager();
        waterMeter.waterLevels[2] = 0;
    }
}
