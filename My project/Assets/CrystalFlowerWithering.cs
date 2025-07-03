using UnityEngine;

public class CrystalFlowerWithering : MonoBehaviour
{
    public WaterMeterUpdated waterMeter;
    public CrystalFlowerUpdated crystalFlower;
    public bool withered;
    public float witherTimerCrystal;
    public float timeToWither;
    public GameObject witherFlowers;
    // Update is called once per frame
    void Update()
    {
        if (waterMeter.waterLevels[2] >= 100 && !withered) 
        {
            withered = true;
            witherTimerCrystal += Time.deltaTime;
            if (witherTimerCrystal >= timeToWither)
            {
                CrystalFlowerWither();
            }
        }
        if (waterMeter.waterLevels[2] < 100)
        {
            
            witherTimerCrystal = 0;
        }
    }
    public void CrystalFlowerWither() 
    {
        crystalFlower.ResetManager();
        waterMeter.waterLevels[2] = 0;
        waterMeter.waterSliders[2].value = 0;
        foreach (var parent in crystalFlower.seedParents) 
        {
            Instantiate(witherFlowers, parent.transform.position, Quaternion.identity, parent.transform);
        }
        crystalFlower.firstStage = true;
    }
}
