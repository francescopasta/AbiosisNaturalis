using UnityEngine;

public class FireFlowerWither : MonoBehaviour
{
    public WaterMeterUpdated waterMeter;
    public FireFlowerUpdated fireflower;
    public bool witheringCrystal;
    public float witherTimerCrystal;
    public float timeToWither;
    public GameObject witherFlowers;
    // Update is called once per frame
    void Update()
    {
        if (waterMeter.waterLevels[1] >= 100)
        {
            witherTimerCrystal += Time.deltaTime;
            if (witherTimerCrystal >= timeToWither)
            {
                CrystalFlowerWither();
            }
        }
        if (waterMeter.waterLevels[1] < 100)
        {
            witheringCrystal = false;
            witherTimerCrystal = 0;
        }
    }
    public void CrystalFlowerWither()
    {
        fireflower.ResetManager();
        waterMeter.waterLevels[1] = 0;
        waterMeter.waterSliders[1].value = 0;
        foreach (var parent in fireflower.seedParents)
        {
            Instantiate(witherFlowers, parent.transform.position, Quaternion.identity, parent.transform);
        }
        fireflower.firstStage = true;
    }
}
