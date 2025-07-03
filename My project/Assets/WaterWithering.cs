using UnityEngine;

public class WaterWithering : MonoBehaviour
{
    public WaterMeterUpdated waterMeter;
    public WaterFlowerUpdated waterFlower;
    public bool witheringCrystal;
    public float witherTimerCrystal;
    public float timeToWither;
    public GameObject witherFlowers;
    // Update is called once per frame
    void Update()
    {
        if (waterMeter.waterLevels[0] >= 100)
        {
            witherTimerCrystal += Time.deltaTime;
            if (witherTimerCrystal >= timeToWither)
            {
                CrystalFlowerWither();
            }
        }
        if (waterMeter.waterLevels[0] < 100)
        {
            witheringCrystal = false;
            witherTimerCrystal = 0;
        }
    }
    public void CrystalFlowerWither()
    {
        waterFlower.ResetManager();
        waterMeter.waterLevels[0] = 0;
        waterMeter.waterSliders[0].value = 0;
        foreach (var parent in waterFlower.seedParents)
        {
            Instantiate(witherFlowers, parent.transform.position, Quaternion.identity, parent.transform);
        }

        // <-- ADD THIS LINE
        waterFlower.firstStage = true;
    }
}
