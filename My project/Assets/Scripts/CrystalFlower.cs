using UnityEngine;

public class CrystalFlower : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that will spawn the crystal flowers.   " +
        "2. Drag the assets the will be used as crystal flower varients under _CrystalSecondStage_ and _CrystalThirdStage_.  " +
        "3. Drag the asset the will be used as the crystal seed and place it under _Seed_.   " +
        "4. Configure the _CrystalSeeds_.   " +
        "5. Configure the _Reward_.  " +
        "6. DO NOT CHANGE ANY OF THE OTHER VARIABLES!!!   " +
        "7. For more information, hover your mouse over the variables. " +
        "8. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    [Tooltip("Array to store all of the varients of the crystal flower for the second stage")]
    public GameObject[] crystalSecondStage;
    [Tooltip("Array to store all of the varients of the crystal flower for the third stage")]
    public GameObject[] crystalThirdStage;

    [Tooltip("Here it goes the asset for the crystal seed")]
    public GameObject seed;

    [Tooltip("This is the amound of crystal seeds that the player will start with")]
    public int crystalSeeds;
    [Tooltip("This is the amound of crystal seeds that the player will will be rewardeda after harvest. Check the comment on _FlowerManager_ line 155 for more information")]
    public int reward;

    public bool firstStage = false;
    public bool secondStage = false;
    public bool thirdStage = false;

    [Tooltip("How many seeds were planted. Used mainly for debugging.")]
    public int plantedSeedCount = 0;
    int spawnedSecondStage = 0; //how many assest are spawned for the second stage
    int spawnedThirdStage = 0; //how many assest are spawned for the third stage

    public void CrystalFlowers()
    {
        if (crystalSecondStage.Length != crystalThirdStage.Length)
        {
            Debug.LogError("Second and Third stage are missmatched!");
            return;
        }

        int index = Random.Range(0, crystalSecondStage.Length);

        GameObject selectedPrimary = crystalSecondStage[index];
        GameObject selectedSecondary = crystalThirdStage[index];

        if (firstStage && crystalSeeds > 0)
        {
            Instantiate(seed, transform.position + Vector3.up * 2f, Quaternion.identity);

            crystalSeeds -= 1;
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
                Vector3 spawnPos = transform.position + offsets[i] + Vector3.up * 2f;
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
