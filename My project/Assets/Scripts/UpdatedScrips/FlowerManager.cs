using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.WSA;

public class FlowerManagerUpdate : MonoBehaviour ///FIX THE WATER SLIDER FOR THE AUTOMATION
{
    [Tooltip(
        "1. Place this script on the object that will manage all the flowers.   " +
        "2. Drag the object the will be used as the planet and place in under _Planet_.  " +
        "3. Do the same for the objects with the _WaterFlowe / FireFlower / CrystalFlower_ scripts.   " +
        "4. Do the same with the object with the _WaterMeter_ script.   " +
        "5. Drag the object that have the _ShopManager_ scrip on.   " +
        "5. Using the sliders below, define the interaction area. Hover your mouse over them for more information.  " +
        "6. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    public GameObject planet;

    public WaterFlowerUpdated waterFlower;
    public FireFlowerUpdated fireFlower;
    public CrystalFlowerUpdated crystalFlower;

    public WaterMeterUpdated waterMeter;

    public ShopManagement shopManagement;

    [Tooltip("This is the left part of the interaction zone")]
    [Range(0f, 1f)] public float minInteractibleArea; 
    [Tooltip("This is the right part of the interaction zone")]
    [Range(0f, 1f)] public float maxInteractibleArea;

    bool nextStage = false;
    public enum FlowerProgress
    {
        None, 
        FirstWait,
        SecondStage,
        SecondWait,
        ThirdStage
    }
    FlowerProgress waterProgress = FlowerProgress.None;
    FlowerProgress fireProgress = FlowerProgress.None;
    FlowerProgress crystalProgress = FlowerProgress.None;

    [System.Serializable]
    public class FlowerAction
    {
        public float targetAngle;
        public System.Action action; //holds information for what the flower should do
        public int index;
    }
    
    public class FlowerBool
    {
        public System.Func<bool> boolCheck; //checks the bool all the time and it's value is not fixed
        public System.Action action; //holds information for what the flower should do
    }

    public List<FlowerAction> flowerActions;
    public List<FlowerBool> flowerBools;



    void Start()
    {


        flowerActions = new List<FlowerAction>
        {
            new FlowerAction //water flower
            {
                index = 0,
                targetAngle = 120f,
                action = () =>
                {

                    if (waterFlower.waterSeeds > 0 && waterProgress == FlowerProgress.None && waterMeter.waterLevels[0] == 0)
                    {
                        waterFlower.firstStage = true;
                    }

                    waterFlower.WaterFlowers();

                    if (waterMeter.waterLevels[0] >= 100 && !waterMeter.waterPlantAutomated)
                    {
                        nextStage = true;
                        WaterFlowerManager();

                    }
                    else if (waterMeter.waterLevels[0] >= 100 && waterMeter.waterPlantAutomated)
                    {
                        WaterFlowerManager();
                    }
                    if (waterFlower.firstStage && !waterMeter.waterPlantAutomated || waterFlower.secondStage && !waterMeter.waterPlantAutomated|| waterFlower.thirdStage && !waterMeter.waterPlantAutomated)
                    {
                        waterMeter.Gain(0);
                    }

                }
            },


            new FlowerAction //fire flower
            {
                index = 1,
                targetAngle = 0f,
                action = () =>
                {
                    if (fireFlower.fireSeeds > 0 && fireProgress == FlowerProgress.None && waterMeter.waterLevels[1] == 0)
                    {
                        fireFlower.firstStage = true;
                    }

                    fireFlower.FireFlowers();
                    if (waterMeter.waterLevels[1] >= 100 && !waterMeter.firePlantAutomated)
                    {
                        nextStage = true;
                        FireFlowerManager();
                    }
                    else if (waterMeter.waterLevels[0] >= 100 && waterMeter.firePlantAutomated)
                    {
                        FireFlowerManager();
                    }
                    if (fireFlower.firstStage && !waterMeter.firePlantAutomated || fireFlower.secondStage && !waterMeter.firePlantAutomated|| fireFlower.thirdStage && !waterMeter.firePlantAutomated)
                    {
                        waterMeter.Gain(1);
                    }
                }
            },

            new FlowerAction //crystal flower
            {
                index = 2,
                targetAngle = -120f,
                action = () =>
                {
                    if (crystalFlower.crystalSeeds > 0 && crystalProgress == FlowerProgress.None && waterMeter.waterLevels[2] == 0)
                    {
                        crystalFlower.firstStage = true;
                    }

                    crystalFlower.CrystalFlowers();

                    if (waterMeter.waterLevels[2] >= 100 && !waterMeter.crystalPlantAutomated)
                    {
                        nextStage = true;
                        CrystalFlowerManager();

                    }
                    else if (waterMeter.waterLevels[2] >= 100 && waterMeter.crystalPlantAutomated)
                    {
                        CrystalFlowerManager();
                    }
                    if (crystalFlower.firstStage && !waterMeter.crystalPlantAutomated || crystalFlower.secondStage && !waterMeter.crystalPlantAutomated|| crystalFlower.thirdStage && ! waterMeter.crystalPlantAutomated)
                    {
                        waterMeter.Gain(2);
                    }
                }
            }
        };


    }


    public void WaterAction() 
    {

        if (waterFlower.waterSeeds > 0 && waterProgress == FlowerProgress.None && waterMeter.waterLevels[0] == 0)
        {
            waterFlower.firstStage = true;
        }

        waterFlower.WaterFlowers();

        //if (waterMeter.waterLevels[0] >= 100 && !waterMeter.waterPlantAutomated)
        //{
        //    nextStage = true;
        //    WaterFlowerManager();

        //}
        if (waterMeter.waterLevels[0] >= 100 && waterMeter.waterPlantAutomated)
        {
            WaterFlowerManager();
        }
        
        
    }

    public void FireAction()
    {
        if (fireFlower.fireSeeds > 0 && fireProgress == FlowerProgress.None && waterMeter.waterLevels[1] == 0)
        {
            fireFlower.firstStage = true;
        }

        fireFlower.FireFlowers();
        //if (waterMeter.waterLevels[1] >= 100 && !waterMeter.firePlantAutomated)
        //{
        //    nextStage = true;
        //    FireFlowerManager();
        //}
        if (waterMeter.waterLevels[1] >= 100 && waterMeter.firePlantAutomated)
        {
            FireFlowerManager();
        }
        
    }
    public void CrystalAction()
    {
        if (crystalFlower.crystalSeeds > 0 && crystalProgress == FlowerProgress.None && waterMeter.waterLevels[2] == 0)
        {
            crystalFlower.firstStage = true;
        }

        crystalFlower.CrystalFlowers();

        //if (waterMeter.waterLevels[2] >= 100 && !waterMeter.crystalPlantAutomated)
        //{
        //    nextStage = true;
        //    CrystalFlowerManager();

        //}
        if (waterMeter.waterLevels[2] >= 100 && waterMeter.crystalPlantAutomated)
        {
            CrystalFlowerManager();
        }
        
    }
    public void WaterFlowerManager() 
    {
        if (waterFlower.firstStage && waterMeter.waterLevels[0] >= 100 && waterMeter.waterPlantAutomated ||
        waterFlower.firstStage && waterMeter.waterLevels[0] >= 100 && nextStage)
        {
            waterFlower.secondStage = true;
            waterFlower.firstStage = false;
            waterProgress = FlowerProgress.SecondStage;
            nextStage = false;
            waterMeter.WaterReset(0);
        }
        else if (waterFlower.secondStage && waterMeter.waterLevels[0] >= 100 && waterProgress == FlowerProgress.SecondStage && waterMeter.waterPlantAutomated ||
        waterFlower.secondStage && waterMeter.waterLevels[0] >= 100 && nextStage)
        {
            waterFlower.thirdStage = true;
            waterFlower.secondStage = false;
            waterProgress = FlowerProgress.ThirdStage;
            nextStage = false;
            waterMeter.WaterReset(0);
        }
        else if (waterFlower.thirdStage && waterMeter.waterLevels[0] >= 100 && waterProgress == FlowerProgress.ThirdStage && waterMeter.waterPlantAutomated ||
        waterFlower.thirdStage && waterMeter.waterLevels[0] >= 100 && nextStage)
        {
            waterFlower.thirdStage = false;
            waterProgress = FlowerProgress.None;
            waterFlower.waterSeeds += waterFlower.rewardSeeds;
            shopManagement.totalCurrency += shopManagement.currencyPerWater * waterFlower.currentThirdStage.Count;
            foreach (var obj in waterFlower.currentThirdStage)
            {
                if (obj != null)
                    Destroy(obj);
            }
            waterFlower.currentThirdStage.Clear();
            waterFlower.nextSeedIndex = 0;
            waterFlower.plantedSeedCount = 0;
            waterFlower.spawnedSecondStage = 0;
            waterFlower.spawnedThirdStage = 0;
            nextStage = false;
            waterMeter.WaterReset(0);
        }


    }
    public void FireFlowerManager()
    {
        if (fireFlower.firstStage && waterMeter.waterLevels[1] >= 100 && waterMeter.firePlantAutomated ||
        fireFlower.firstStage && waterMeter.waterLevels[1] >= 100 && nextStage)
        {
            fireFlower.secondStage = true;
            fireFlower.firstStage = false;
            fireProgress = FlowerProgress.SecondStage;
            nextStage = false;
            waterMeter.WaterReset(1);
        }
        else if (fireFlower.secondStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.SecondStage && waterMeter.firePlantAutomated ||
         fireFlower.secondStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.SecondStage  && nextStage)
        {
            fireFlower.thirdStage = true;
            fireFlower.secondStage = false;
            fireProgress = FlowerProgress.ThirdStage;
            nextStage = false;
            waterMeter.WaterReset(1);
        }
        else if (fireFlower.thirdStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.ThirdStage && waterMeter.firePlantAutomated ||
         fireFlower.thirdStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.ThirdStage && nextStage)
        {
            fireFlower.thirdStage = false;
            fireProgress = FlowerProgress.None;
            fireFlower.fireSeeds += fireFlower.rewardSeeds;
            shopManagement.totalCurrency += shopManagement.currencyPerFire * fireFlower.currentThirdStage.Count;
            foreach (var obj in fireFlower.currentThirdStage)
            {
                if (obj != null)
                    Destroy(obj);
            }
            fireFlower.currentThirdStage.Clear();
            fireFlower.nextSeedIndex = 0;
            fireFlower.plantedSeedCount = 0;
            fireFlower.spawnedSecondStage = 0;
            fireFlower.spawnedThirdStage = 0;
            nextStage = false;
            waterMeter.WaterReset(1);
        }


    }
    public void CrystalFlowerManager()
    {
        if (crystalFlower.firstStage && waterMeter.waterLevels[2] >= 100 && waterMeter.crystalPlantAutomated ||
        crystalFlower.firstStage && waterMeter.waterLevels[2] >= 100 && nextStage)
        {
            crystalFlower.secondStage = true;
            crystalFlower.firstStage = false;
            crystalProgress = FlowerProgress.SecondStage;
            nextStage = false;
            waterMeter.WaterReset(2);
        }
        else if (crystalFlower.secondStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.SecondStage && waterMeter.crystalPlantAutomated ||
        crystalFlower.secondStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.SecondStage && nextStage)
        {
            crystalFlower.thirdStage = true;
            crystalFlower.secondStage = false;
            crystalProgress = FlowerProgress.ThirdStage;
            nextStage = false;  
            waterMeter.WaterReset(2);
        }
        else if (crystalFlower.thirdStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.ThirdStage && waterMeter.crystalPlantAutomated ||
        crystalFlower.thirdStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.ThirdStage && nextStage)
        {
            crystalFlower.thirdStage = false;
            crystalProgress = FlowerProgress.None;
            crystalFlower.crystalSeeds += crystalFlower.rewardSeeds;
            shopManagement.totalCurrency += shopManagement.currencyPerCrystal * crystalFlower.currentThirdStage.Count;
            foreach (var obj in crystalFlower.currentThirdStage)
            {
                if (obj != null)
                    Destroy(obj);
            }
            crystalFlower.currentThirdStage.Clear();
            crystalFlower.nextSeedIndex = 0;
            crystalFlower.plantedSeedCount = 0;
            crystalFlower.spawnedSecondStage = 0;
            crystalFlower.spawnedThirdStage = 0;
            waterMeter.WaterReset(2);
        }


    }
    void Update()
    {
        ///Chooses which code to run, depanding on the build
        #if UNITY_EDITOR || UNITY_STANDALONE //runs before the game begin
        ComputerInteraction();
#elif UNITY_IOS || UNITY_ANDROID
            MobileInteraction();
#endif

        if (waterMeter.waterPlantAutomated)
        {
            WaterAction();
        }
        if (waterMeter.firePlantAutomated)
        {
            FireAction();
        }
        if (waterMeter.crystalPlantAutomated)
        {
            CrystalAction();
        }
        // AutomateGarden();

        //foreach (var condition in flowerBools)
        //{
        //    if (condition.boolCheck())
        //    {
        //        condition.action();
        //    }
        //}
    }

    void ComputerInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float x = Input.mousePosition.x;
            float screenWidth = Screen.width;

            if (x > screenWidth * minInteractibleArea && x < screenWidth * maxInteractibleArea) //compares the mouse position to the interactible area
            {
                float angleZ = NormalizeAngle(planet.transform.localEulerAngles.z);

                foreach (var flower in flowerActions)
                {
                    if (Mathf.Abs(angleZ - flower.targetAngle) <= 1f)
                    {
                        flower.action.Invoke();
                        int flowerIndex = flower.index;
                       
                        
                        break;
                    }
                }
            }
        }
    } 

    void MobileInteraction()
    {
        if (Input.touchCount > 0) //cheks if there is a finger on the screen
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began: //began touching
                    float x = touch.position.x;
                    float screenWidth = Screen.width;

                    if (x > screenWidth * minInteractibleArea && x < screenWidth * maxInteractibleArea) //compares the finger position to the interactible area
                    {
                        float angleZ = NormalizeAngle(planet.transform.localEulerAngles.z);

                        foreach (var flower in flowerActions)
                        {
                            if (Mathf.Abs(angleZ - flower.targetAngle) <= 1f)
                            {
                                flower.action.Invoke();
                                int flowerIndex = flower.index;


                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    public void AutomateGarden(int FlowerIndex)
    {
        if (FlowerIndex == 0)
        {
            waterMeter.waterPlantAutomated = true;
        }
        else if (FlowerIndex == 1)
        {
            waterMeter.firePlantAutomated = true ;
        }
        else if (FlowerIndex == 2)
        {
            waterMeter.crystalPlantAutomated= true ;
        }

    }
}
// flowerBools = new List<FlowerBool>
        //{
        //#region Water
        //    new FlowerBool //water flower second stage
        //    {
        //        boolCheck = () => waterFlower.firstStage && waterMeter.waterLevels[0] >= 100 && waterMeter.waterPlantAutomated || 
        //         waterFlower.firstStage && waterMeter.waterLevels[0] >= 100 && nextStage,
        //        action = () =>
        //        {
        //            waterFlower.secondStage = true;
        //            waterFlower.firstStage = false;
        //            waterProgress = FlowerProgress.SecondStage;
        //            nextStage = false;
        //            waterMeter.WaterReset(0);
        //        }

//    },

//    //new FlowerBool //wait between stage 2 and 3
//    //{
//    //    boolCheck = () => waterProgress == FlowerProgress.FirstWait && waterMeter.waterLevels[0] == 0,
//    //    action = () =>
//    //    {
//    //        waterProgress = FlowerProgress.SecondStage;
//    //    }

//    //},

//    new FlowerBool //water flower third stage
//    {
//        boolCheck = () => waterFlower.secondStage && waterMeter.waterLevels[0] >= 100 && waterProgress == FlowerProgress.SecondStage && waterMeter.waterPlantAutomated ||
//         waterFlower.secondStage && waterMeter.waterLevels[0] >= 100 && nextStage,
//        action = () =>
//        {
//            waterFlower.thirdStage = true;
//            waterFlower.secondStage = false;
//            waterProgress = FlowerProgress.ThirdStage;
//            nextStage = false;
//            waterMeter.WaterReset(0);

//        }
//    },

//    //new FlowerBool //wait between stage 3 and harvest
//    //{
//    //    boolCheck = () => waterProgress == FlowerProgress.SecondWait && waterMeter.waterLevels[0] == 0,
//    //    action = () =>
//    //    {
//    //        waterProgress = FlowerProgress.ThirdStage;
//    //    }
//    //},

//    new FlowerBool //water flower harvest
//    {
//        boolCheck = () => waterFlower.thirdStage && waterMeter.waterLevels[0] >= 100 && waterProgress == FlowerProgress.ThirdStage && waterMeter.waterPlantAutomated ||
//         waterFlower.thirdStage && waterMeter.waterLevels[0] >= 100 && nextStage,
//        action = () =>
//        { 
//            waterFlower.thirdStage = false;
//            waterProgress = FlowerProgress.None;
//            waterFlower.waterSeeds += waterFlower.rewardSeeds;
//            shopManagement.totalCurrency += shopManagement.currencyPerWater * waterFlower.currentThirdStage.Count;
//            foreach (var obj in waterFlower.currentThirdStage)
//            {
//                if(obj != null)
//                Destroy(obj);
//            }
//            waterFlower.currentThirdStage.Clear();
//            waterFlower.nextSeedIndex = 0;
//            waterFlower.plantedSeedCount = 0;
//            waterFlower.spawnedSecondStage = 0;
//            waterFlower.spawnedThirdStage = 0;
//            nextStage = false;
//            waterMeter.WaterReset(0);
//        }
//    },
//#endregion

//#region Crystal
//    new FlowerBool //crystal flower second stage
//    {
//        boolCheck = () => crystalFlower.firstStage && waterMeter.waterLevels[2] >= 100,
//        action = () =>
//        {
//            crystalFlower.secondStage = true;
//            crystalFlower.firstStage = false;
//            crystalProgress = FlowerProgress.SecondStage;
//            waterMeter.WaterReset(2);
//        }

//    },

//    //new FlowerBool //wait between stage 2 and 3
//    //{
//    //    boolCheck = () => crystalProgress == FlowerProgress.FirstWait && waterMeter.waterLevels[2] == 0,
//    //    action = () =>
//    //    {
//    //        crystalProgress = FlowerProgress.SecondStage;
//    //    }

//    //},

//    new FlowerBool //crystal flower third stage
//    {
//        boolCheck = () => crystalFlower.secondStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.SecondStage,
//        action = () =>
//        {
//            crystalFlower.thirdStage = true;
//            crystalFlower.secondStage = false;
//            crystalProgress = FlowerProgress.ThirdStage;
//            waterMeter.WaterReset(2);
//        }
//    },

//    //new FlowerBool //wait between stage 3 and harvest
//    //{
//    //    boolCheck = () => crystalProgress == FlowerProgress.SecondWait && waterMeter.waterLevels[2] == 0,
//    //    action = () =>
//    //    {
//    //        crystalProgress = FlowerProgress.ThirdStage;
//    //    }
//    //},

//    new FlowerBool //crystal flower harvest
//    {
//        boolCheck = () => crystalFlower.thirdStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.ThirdStage,
//        action = () =>
//        {
//            crystalFlower.thirdStage = false;
//            crystalProgress = FlowerProgress.None;
//            crystalFlower.crystalSeeds += crystalFlower.rewardSeeds;
//            shopManagement.totalCurrency += shopManagement.currencyPerCrystal * crystalFlower.currentThirdStage.Count;
//            foreach(var obj in crystalFlower.currentThirdStage)
//            {
//                if(obj != null)
//                Destroy(obj);
//            }
//            crystalFlower.currentThirdStage.Clear();
//            crystalFlower.nextSeedIndex = 0;
//            crystalFlower.plantedSeedCount = 0;
//            crystalFlower.spawnedSecondStage = 0;
//            crystalFlower.spawnedThirdStage = 0;
//            waterMeter.WaterReset(2);
//        }
//    },

//    #endregion

//#region Fire
//    new FlowerBool //fire flower second stage
//    {
//        boolCheck = () => fireFlower.firstStage && waterMeter.waterLevels[1] >= 100 && waterMeter.firePlantAutomated ||
//        fireFlower.firstStage && waterMeter.waterLevels[1] >= 100 && nextStage,
//        action = () =>
//                {
//                    fireFlower.secondStage = true;
//                    fireFlower.firstStage = false;
//                    fireProgress = FlowerProgress.SecondStage;
//                    nextStage = false;
//                    waterMeter.WaterReset(1);
//                }

//            },

//            //new FlowerBool //wait between stage 2 and 3
//            //{
//            //    boolCheck = () => fireProgress == FlowerProgress.FirstWait && waterMeter.waterLevels[1] == 0,
//            //    action = () =>
//            //    {
//            //        fireProgress = FlowerProgress.SecondStage;
//            //    }

//            //},

//            new FlowerBool //fire flower third stage
//            {
//                boolCheck = () => fireFlower.secondStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.SecondStage  && waterMeter.firePlantAutomated ||
//                fireFlower.secondStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.SecondStage  && nextStage,
//                action = () =>
//                {
//                    fireFlower.thirdStage = true;
//                    fireFlower.secondStage = false;
//                    fireProgress = FlowerProgress.ThirdStage;
//                    nextStage = false;
//                    waterMeter.WaterReset(1);
//                }
//            },

//            //new FlowerBool //wait between stage 3 and harvest
//            //{
//            //    boolCheck = () => fireProgress == FlowerProgress.SecondWait && waterMeter.waterLevels[1] == 0,
//            //    action = () =>
//            //    {
//            //        fireProgress = FlowerProgress.ThirdStage;
//            //    }
//            //},

//            new FlowerBool //fire flower harvest
//            {
//                boolCheck = () => fireFlower.thirdStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.ThirdStage && waterMeter.firePlantAutomated ||
//                fireFlower.thirdStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.ThirdStage && nextStage,
//                action = () =>
//                {
//                    fireFlower.thirdStage = false;
//                    fireProgress = FlowerProgress.None;
//                    fireFlower.fireSeeds += fireFlower.rewardSeeds;
//                    shopManagement.totalCurrency += shopManagement.currencyPerFire * fireFlower.currentThirdStage.Count;
//                    foreach(var obj in fireFlower.currentThirdStage)
//                    {
//                        if(obj != null)
//                        Destroy(obj);
//                    }
//                    fireFlower.currentThirdStage.Clear();
//                    fireFlower.nextSeedIndex = 0;
//                    fireFlower.plantedSeedCount = 0;
//                    fireFlower.spawnedSecondStage = 0;
//                    fireFlower.spawnedThirdStage = 0;
//                    nextStage = false;
//                    waterMeter.WaterReset(1);
//                }
//            },

//#endregion
//        };