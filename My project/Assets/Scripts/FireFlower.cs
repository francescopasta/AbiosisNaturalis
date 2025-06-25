using UnityEngine;

public class FireFlower : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that will spawn the fire flowers.   " +
        "2. Drag the assets the will be used as fire flower varients under _FireSecondStage_ and _FireThirdStage_.  " +
        "3. Drag the asset the will be used as the fire seed and place it under _Seed_.   " +
        "4. Configure the _FireSeeds_.   " +
        "5. Configure the _Reward_.  " +
        "6. DO NOT CHANGE ANY OF THE OTHER VARIABLES!!!   " +
        "7. For more information, hover your mouse over the variables. " +
        "8. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    [Tooltip("Array to store all of the varients of the fire flower for the second stage")]
    public GameObject[] fireSecondStage;
    [Tooltip("Array to store all of the varients of the fire flower for the third stage")]
    public GameObject[] fireThirdStage;

    [Tooltip("Here it goes the asset for the fire seed")]
    public GameObject seed;

    [Tooltip("This is the amound of fire seeds that the player will start with")]
    public int fireSeeds;
    [Tooltip("This is the amound of fire seeds that the player will will be rewardeda after harvest. Check the comment on _FlowerManager_ line 155 for more information")]
    public int reward;

    public bool firstStage = false;
    public bool secondStage = false;
    public bool thirdStage = false;

    [Tooltip("How many seeds were planted. Used mainly for debugging.")]
    public int plantedSeedCount = 0;
    int spawnedSecondStage = 0; //how many assest are spawned for the second stage
    int spawnedThirdStage = 0; //how many assest are spawned for the third stage

    public void FireFlowers()
    {
        if (fireSecondStage.Length != fireThirdStage.Length)
        {
            Debug.LogError("Second and Third stage are missmatched!");
            return;
        }

        int index = Random.Range(0, fireSecondStage.Length);

        GameObject selectedPrimary = fireSecondStage[index];
        GameObject selectedSecondary = fireThirdStage[index];

        if (firstStage && fireSeeds > 0)
        {
            Instantiate(seed, Vector3.zero, Quaternion.identity);

            fireSeeds -= 1;
            plantedSeedCount += 1;
            Debug.Log("First stage water placed");
        }

        if (secondStage && plantedSeedCount > 0 && spawnedSecondStage < plantedSeedCount)
        {
            Vector3[] offsets = {
            Vector3.zero,
            new Vector3(-2, 0, 0),
            new Vector3(2, 0, 0)
            };

            for (int i = 0; i < plantedSeedCount && i < offsets.Length; i++)
            {
                Vector3 spawnPos = transform.position + offsets[i];
                Instantiate(selectedPrimary, spawnPos, Quaternion.identity);
                spawnedSecondStage++;
            }

            Debug.Log("Second stage placed");
        }

        if (thirdStage && plantedSeedCount > 0 && spawnedThirdStage < plantedSeedCount)
        {
            Vector3[] offsets = {
            Vector3.zero,
            new Vector3(-2, 0, 0),
            new Vector3(2, 0, 0)
            };

            for (int i = 0; i < plantedSeedCount && i < offsets.Length; i++)
            {
                Vector3 spawnPos = transform.position + offsets[i];
                Instantiate(selectedSecondary, spawnPos, Quaternion.identity);
                spawnedThirdStage++;
            }

            Debug.Log("Third stage placed");
        }
    }
}
