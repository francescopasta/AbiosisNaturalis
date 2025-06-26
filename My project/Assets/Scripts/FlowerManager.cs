using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that will manage all the flowers.   " +
        "2. Drag the object the will be used as the planet and place in under _Planet_.  " +
        "3. Do the same for the objects with the _WaterFlowe / FireFlower / CrystalFlower_ scripts.   " +
        "4. Do the same with the object with the _WaterMeter_ script.   " +
        "5. Using the sliders below, define the interaction area. Hover your mouse over them for more information.  " +
        "6. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    public GameObject planet;

    public WaterFlower waterFlower;
    public FireFlower fireFlower;
    public CrystalFlower crystalFlower;

    public WaterMeter waterMeter;

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

    public class FlowerAction
    {
        public float targetAngle;
        public System.Action action; //holds information for what the flower should do
    }

    public class FlowerBool
    {
        public System.Func<bool> boolCheck; //checks the bool all the time and it's value is not fixed
        public System.Action action; //holds information for what the flower should do
    }

    List<FlowerAction> flowerActions;
    List<FlowerBool> flowerBools;

    void Start()
    {
        FlowerProgress flowerProgress = FlowerProgress.None;

        flowerActions = new List<FlowerAction>
        {
            new FlowerAction //water flower
            {
                targetAngle = 90f,
                action = () =>
                {
                    if (waterFlower.waterSeeds > 0)
                    {
                        waterFlower.firstStage = true;
                    }

                    waterFlower.WaterFlowers();
                }
            },

            new FlowerAction //crystal flower
            {
                targetAngle = -90f,
                action = () =>
                {
                    if (crystalFlower.crystalSeeds > 0)
                    {
                        crystalFlower.firstStage = true;
                    }

                    crystalFlower.CrystalFlowers();
                }
            },

            new FlowerAction //fire flower
            {
                targetAngle = 0f,
                action = () =>
                {
                    if (fireFlower.fireSeeds > 0)
                    {
                        fireFlower.firstStage = true;
                    }

                    fireFlower.FireFlowers();
                }
            }
        };

        flowerBools = new List<FlowerBool>
        {
        #region Water
            new FlowerBool //water flower second stage
            {
                boolCheck = () => waterFlower.firstStage && waterMeter.water >= 100,
                action = () =>
                {
                    waterFlower.secondStage = true;
                    waterFlower.firstStage = false;
                    flowerProgress = FlowerProgress.FirstWait;
                }

            },

            new FlowerBool //wait between stage 2 and 3
            {
                boolCheck = () => flowerProgress == FlowerProgress.FirstWait && waterMeter.water == 0,
                action = () =>
                {
                    flowerProgress = FlowerProgress.SecondStage;
                }

            },

            new FlowerBool //water flower third stage
            {
                boolCheck = () => waterFlower.secondStage && waterMeter.water >= 100 && flowerProgress == FlowerProgress.SecondStage,
                action = () =>
                {
                    waterFlower.thirdStage = true;
                    waterFlower.secondStage = false;
                    flowerProgress = FlowerProgress.SecondWait;
                }
            },

            new FlowerBool //wait between stage 3 and harvest
            {
                boolCheck = () => flowerProgress == FlowerProgress.SecondWait && waterMeter.water == 0,
                action = () =>
                {
                    flowerProgress = FlowerProgress.ThirdStage;
                }
            },

            new FlowerBool //water flower harvest
            {
                boolCheck = () => waterFlower.thirdStage && waterMeter.water >= 100 && flowerProgress == FlowerProgress.ThirdStage,
                action = () =>
                {
                    waterFlower.thirdStage = false;
                    flowerProgress = FlowerProgress.None;
                    waterFlower.waterSeeds += waterFlower.reward; //this is here for the moment and for closing the loop for testing purposes. it could be removed later, if the player buys seeds from the shop
                    //the currency goes up
                }
            },
            #endregion

        #region Crystal
            new FlowerBool //crystal flower second stage
            {
                boolCheck = () => crystalFlower.firstStage && waterMeter.water >= 100,
                action = () =>
                {
                    crystalFlower.secondStage = true;
                    crystalFlower.firstStage = false;
                    flowerProgress = FlowerProgress.FirstWait;
                }

            },

            new FlowerBool //wait between stage 2 and 3
            {
                boolCheck = () => flowerProgress == FlowerProgress.FirstWait && waterMeter.water == 0,
                action = () =>
                {
                    flowerProgress = FlowerProgress.SecondStage;
                }

            },

            new FlowerBool //crystal flower third stage
            {
                boolCheck = () => crystalFlower.secondStage && waterMeter.water >= 100 && flowerProgress == FlowerProgress.SecondStage,
                action = () =>
                {
                    crystalFlower.thirdStage = true;
                    crystalFlower.secondStage = false;
                    flowerProgress = FlowerProgress.SecondWait;
                }
            },

            new FlowerBool //wait between stage 3 and harvest
            {
                boolCheck = () => flowerProgress == FlowerProgress.SecondWait && waterMeter.water == 0,
                action = () =>
                {
                    flowerProgress = FlowerProgress.ThirdStage;
                }
            },

            new FlowerBool //crystal flower harvest
            {
                boolCheck = () => crystalFlower.thirdStage && waterMeter.water >= 100 && flowerProgress == FlowerProgress.ThirdStage,
                action = () =>
                {
                    crystalFlower.thirdStage = false;
                    flowerProgress = FlowerProgress.None;
                    crystalFlower.crystalSeeds += crystalFlower.reward; //this is here for the moment and for closing the loop for testing purposes. it could be removed later, if the player buys seeds from the shop
                    //the currency goes up
                }
            },

            #endregion

        #region Fire
            new FlowerBool //fire flower second stage
            {
                boolCheck = () => fireFlower.firstStage && waterMeter.water >= 100,
                action = () =>
                {
                    fireFlower.secondStage = true;
                    fireFlower.firstStage = false;
                    flowerProgress = FlowerProgress.FirstWait;
                }

            },

            new FlowerBool //wait between stage 2 and 3
            {
                boolCheck = () => flowerProgress == FlowerProgress.FirstWait && waterMeter.water == 0,
                action = () =>
                {
                    flowerProgress = FlowerProgress.SecondStage;
                }

            },

            new FlowerBool //fire flower third stage
            {
                boolCheck = () => fireFlower.secondStage && waterMeter.water >= 100 && flowerProgress == FlowerProgress.SecondStage,
                action = () =>
                {
                    fireFlower.thirdStage = true;
                    fireFlower.secondStage = false;
                    flowerProgress = FlowerProgress.SecondWait;
                }
            },

            new FlowerBool //wait between stage 3 and harvest
            {
                boolCheck = () => flowerProgress == FlowerProgress.SecondWait && waterMeter.water == 0,
                action = () =>
                {
                    flowerProgress = FlowerProgress.ThirdStage;
                }
            },

            new FlowerBool //fire flower harvest
            {
                boolCheck = () => fireFlower.thirdStage && waterMeter.water >= 100 && flowerProgress == FlowerProgress.ThirdStage,
                action = () =>
                {
                    fireFlower.thirdStage = false;
                    flowerProgress = FlowerProgress.None;
                    fireFlower.fireSeeds += fireFlower.reward; //this is here for the moment and for closing the loop for testing purposes. it could be removed later, if the player buys seeds from the shop
                    //the currency goes up
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

                        break;
                    }
                }

                if (waterMeter.water >= 100)
                {
                    waterMeter.waterGainDefault = 0;
                }

                else if (waterMeter.water == 0)
                {
                    waterMeter.waterGainDefault = waterMeter.waterGainUpdate;
                }

                waterMeter.WaterGain();
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

                                break;
                            }
                        }

                        if (waterMeter.water >= 100)
                        {
                            waterMeter.waterGainDefault = 0;
                        }

                        else if (waterMeter.water == 0)
                        {
                            waterMeter.waterGainDefault = waterMeter.waterGainUpdate;
                        }

                        waterMeter.WaterGain();
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

    //for debugging
    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        float screenHeight = Camera.main.pixelHeight;
        float screenWidth = Camera.main.pixelWidth;

        // Get min and max X in screen space
        float minX = screenWidth * minInteractibleArea;
        float maxX = screenWidth * maxInteractibleArea;

        // We'll draw a vertical line from bottom to top of the screen
        Vector3 bottomMin = Camera.main.ScreenToWorldPoint(new Vector3(minX, 0, Camera.main.nearClipPlane + 5f));
        Vector3 topMin = Camera.main.ScreenToWorldPoint(new Vector3(minX, screenHeight, Camera.main.nearClipPlane + 5f));

        Vector3 bottomMax = Camera.main.ScreenToWorldPoint(new Vector3(maxX, 0, Camera.main.nearClipPlane + 5f));
        Vector3 topMax = Camera.main.ScreenToWorldPoint(new Vector3(maxX, screenHeight, Camera.main.nearClipPlane + 5f));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(bottomMin, topMin);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomMax, topMax);
    }

}
