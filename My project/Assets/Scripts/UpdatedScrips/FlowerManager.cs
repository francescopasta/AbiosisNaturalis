using System.Collections.Generic;
using UnityEngine;

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

    public ShopManagementUpdated shopManagement;

    [Tooltip("This is the left part of the interaction zone")]
    [Range(0f, 1f)] public float minInteractibleArea; 
    [Tooltip("This is the right part of the interaction zone")]
    [Range(0f, 1f)] public float maxInteractibleArea; 

    public enum FlowerProgress
    {
        None, 
        FirstWait,
        SecondStage,
        SecondWait,
        ThirdStage
    }

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
    List<FlowerBool> flowerBools;

    void Start()
    {
        FlowerProgress waterProgress = FlowerProgress.None;
        FlowerProgress fireProgress = FlowerProgress.None;
        FlowerProgress crystalProgress = FlowerProgress.None;

        flowerActions = new List<FlowerAction>
        {
            new FlowerAction //water flower
            {
                index = 0,
                targetAngle = 90f,
                action = () =>
                {
                    if (waterFlower.waterSeeds > 0 && waterProgress == FlowerProgress.None && waterMeter.waterLevels[0] == 0)
                    {
                        waterFlower.firstStage = true;
                    }
                
                    waterFlower.WaterFlowers();
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
                }
            },
            
            new FlowerAction //crystal flower
            {
                index = 2,
                targetAngle = -90f,
                action = () =>
                {
                    if (crystalFlower.crystalSeeds > 0 && crystalProgress == FlowerProgress.None && waterMeter.waterLevels[2] == 0)
                    {
                        crystalFlower.firstStage = true;
                    }

                    crystalFlower.CrystalFlowers();
                }
            }
        };

        flowerBools = new List<FlowerBool>
        {
        #region Water
            new FlowerBool //water flower second stage
            {
                boolCheck = () => waterFlower.firstStage && waterMeter.waterLevels[0] >= 100,
                action = () =>
                {
                    waterFlower.secondStage = true;
                    waterFlower.firstStage = false;
                    waterProgress = FlowerProgress.FirstWait;
                }

            },
            
            new FlowerBool //wait between stage 2 and 3
            {
                boolCheck = () => waterProgress == FlowerProgress.FirstWait && waterMeter.waterLevels[0] == 0,
                action = () =>
                {
                    waterProgress = FlowerProgress.SecondStage;
                }

            },

            new FlowerBool //water flower third stage
            {
                boolCheck = () => waterFlower.secondStage && waterMeter.waterLevels[0] >= 100 && waterProgress == FlowerProgress.SecondStage,
                action = () =>
                {
                    waterFlower.thirdStage = true;
                    waterFlower.secondStage = false;
                    waterProgress = FlowerProgress.SecondWait;
                }
            },

            new FlowerBool //wait between stage 3 and harvest
            {
                boolCheck = () => waterProgress == FlowerProgress.SecondWait && waterMeter.waterLevels[0] == 0,
                action = () =>
                {
                    waterProgress = FlowerProgress.ThirdStage;
                }
            },

            new FlowerBool //water flower harvest
            {
                boolCheck = () => waterFlower.thirdStage && waterMeter.waterLevels[0] >= 100 && waterProgress == FlowerProgress.ThirdStage,
                action = () =>
                { 
                    waterFlower.thirdStage = false;
                    waterProgress = FlowerProgress.None;
                    waterFlower.waterSeeds += waterFlower.rewardSeeds;
                    shopManagement.totalCurrency += shopManagement.currencyPerWater * waterFlower.currentThirdStage.Count;
                    foreach (var obj in waterFlower.currentThirdStage)
                    {
                        if(obj != null)
                        Destroy(obj);
                    }
                    waterFlower.currentThirdStage.Clear();
                    waterFlower.nextSeedIndex = 0;
                    waterFlower.plantedSeedCount = 0;
                    waterFlower.spawnedSecondStage = 0;
                    waterFlower.spawnedThirdStage = 0;
                }
            },
            #endregion

        #region Crystal
            new FlowerBool //crystal flower second stage
            {
                boolCheck = () => crystalFlower.firstStage && waterMeter.waterLevels[2] >= 100,
                action = () =>
                {
                    crystalFlower.secondStage = true;
                    crystalFlower.firstStage = false;
                    crystalProgress = FlowerProgress.FirstWait;
                }

            },

            new FlowerBool //wait between stage 2 and 3
            {
                boolCheck = () => crystalProgress == FlowerProgress.FirstWait && waterMeter.waterLevels[2] == 0,
                action = () =>
                {
                    crystalProgress = FlowerProgress.SecondStage;
                }

            },

            new FlowerBool //crystal flower third stage
            {
                boolCheck = () => crystalFlower.secondStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.SecondStage,
                action = () =>
                {
                    crystalFlower.thirdStage = true;
                    crystalFlower.secondStage = false;
                    crystalProgress = FlowerProgress.SecondWait;
                }
            },

            new FlowerBool //wait between stage 3 and harvest
            {
                boolCheck = () => crystalProgress == FlowerProgress.SecondWait && waterMeter.waterLevels[2] == 0,
                action = () =>
                {
                    crystalProgress = FlowerProgress.ThirdStage;
                }
            },

            new FlowerBool //crystal flower harvest
            {
                boolCheck = () => crystalFlower.thirdStage && waterMeter.waterLevels[2] >= 100 && crystalProgress == FlowerProgress.ThirdStage,
                action = () =>
                {
                    crystalFlower.thirdStage = false;
                    crystalProgress = FlowerProgress.None;
                    crystalFlower.crystalSeeds += crystalFlower.rewardSeeds;
                    shopManagement.totalCurrency += shopManagement.currencyPerCrystal * crystalFlower.currentThirdStage.Count;
                    foreach(var obj in crystalFlower.currentThirdStage)
                    {
                        if(obj != null)
                        Destroy(obj);
                    }
                    crystalFlower.currentThirdStage.Clear();
                    crystalFlower.nextSeedIndex = 0;
                    crystalFlower.plantedSeedCount = 0;
                    crystalFlower.spawnedSecondStage = 0;
                    crystalFlower.spawnedThirdStage = 0;
                }
            },

            #endregion

        #region Fire
            new FlowerBool //fire flower second stage
            {
                boolCheck = () => fireFlower.firstStage && waterMeter.waterLevels[1] >= 100,
                action = () =>
                {
                    fireFlower.secondStage = true;
                    fireFlower.firstStage = false;
                    fireProgress = FlowerProgress.FirstWait;
                }

            },

            new FlowerBool //wait between stage 2 and 3
            {
                boolCheck = () => fireProgress == FlowerProgress.FirstWait && waterMeter.waterLevels[1] == 0,
                action = () =>
                {
                    fireProgress = FlowerProgress.SecondStage;
                }

            },

            new FlowerBool //fire flower third stage
            {
                boolCheck = () => fireFlower.secondStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.SecondStage,
                action = () =>
                {
                    fireFlower.thirdStage = true;
                    fireFlower.secondStage = false;
                    fireProgress = FlowerProgress.SecondWait;
                }
            },

            new FlowerBool //wait between stage 3 and harvest
            {
                boolCheck = () => fireProgress == FlowerProgress.SecondWait && waterMeter.waterLevels[1] == 0,
                action = () =>
                {
                    fireProgress = FlowerProgress.ThirdStage;
                }
            },

            new FlowerBool //fire flower harvest
            {
                boolCheck = () => fireFlower.thirdStage && waterMeter.waterLevels[1] >= 100 && fireProgress == FlowerProgress.ThirdStage,
                action = () =>
                {
                    fireFlower.thirdStage = false;
                    fireProgress = FlowerProgress.None;
                    fireFlower.fireSeeds += fireFlower.rewardSeeds;
                    shopManagement.totalCurrency += shopManagement.currencyPerFire * fireFlower.currentThirdStage.Count;
                    foreach(var obj in fireFlower.currentThirdStage)
                    {
                        if(obj != null)
                        Destroy(obj);
                    }
                    fireFlower.currentThirdStage.Clear();
                    fireFlower.nextSeedIndex = 0;
                    fireFlower.plantedSeedCount = 0;
                    fireFlower.spawnedSecondStage = 0;
                    fireFlower.spawnedThirdStage = 0;
                }
            },

#endregion
        };
    }        

    void Update()
    {
        ///Chooses which code to run, depanding on the build
        #if UNITY_EDITOR || UNITY_STANDALONE //runs before the game begin
        ComputerInteraction();
        #elif UNITY_IOS || UNITY_ANDROID
            MobileInteraction();
        #endif

        AutomateGarden();

        foreach (var condition in flowerBools)
        {
            if (condition.boolCheck())
            {
                condition.action();
            }
        }
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

                        foreach (var condtion in flowerBools)
                        {
                            if (condtion.boolCheck())
                            {
                                condtion.action.Invoke();
                            }
                        }

                        int flowerIndex = flower.index;
                        waterMeter.Gain(flowerIndex);

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

                                foreach (var condtion in flowerBools)
                                {
                                    if (condtion.boolCheck())
                                    {
                                        condtion.action.Invoke();
                                    }
                                }

                                int flowerIndex = flower.index; // you’ll need to store this in the flowerAction
                                waterMeter.Gain(flowerIndex);

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

    void AutomateGarden()
    {
        //0 = water, 1 = fire, 2 = crystal 
        for (int i = 0; i < 3; i++)
        {
            if (shopManagement.upgades.Length > i && shopManagement.upgades[i])
            {
                if (i < flowerActions.Count)
                {
                    flowerActions[i].action.Invoke();
                }

                foreach (var condition in flowerBools)
                {
                    if (condition.boolCheck())
                    {
                        condition.action.Invoke();
                    }
                }

                waterMeter.Gain(i);
            }
        }
    }
}
