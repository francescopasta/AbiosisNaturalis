using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerUpdated : MonoBehaviour
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
    [Tooltip("This is the amound of fire seeds that the player will will be rewardeda after harvest. The seeds are purely used for logic purposes. At maximum, it must be the same number as the max number of flowers per garden.")]
    public int rewardSeeds = 3;

    public bool firstStage = false;
    public bool secondStage = false;
    public bool thirdStage = false;

    [Tooltip("How many seeds were planted. Used mainly for debugging.")]
    public int plantedSeedCount = 0;
    public int spawnedSecondStage = 0; //how many assest are spawned for the second stage
    public int spawnedThirdStage = 0; //how many assest are spawned for the third stage

    public int nextSeedIndex = 0;

    public List<GameObject> currentSeeds = new();
    public List<GameObject> currentSecondStage = new();
    public List<GameObject> currentThirdStage = new();

    GameObject selectedPrimary;
    GameObject selectedSecondary;

    public List<GameObject> seedParents = new List<GameObject>();

    public ShopManagement shopManagementUpdated;

    public List<int> flowerIndexes = new List<int>();
    public List<int> positionInts = new List<int>();
    public bool seedsPlanted = false;
    public void FireFlowers()
    {

        if (fireSecondStage.Length != fireThirdStage.Length)
        {
            Debug.LogError("Second and Third stage are missmatched!");
            return;
        }

        //for (int i = 0; i < waterSeeds; i++)
        //{
        //    int index = Random.Range(0, waterSecondStage.Length);

        //    selectedPrimary = waterSecondStage[index];
        //    selectedSecondary = waterThirdStage[index];
        //}

        if (firstStage && nextSeedIndex < 3 && fireSeeds > 0)
        {
            positionInts.Clear();
            seedsPlanted = true;
            fireSeeds = 0;
            StartCoroutine(PlantSeedFirstStage(0.15f));
            // Debug.Log("First stage water placed");
            flowerIndexes.Clear();
        }

        if (secondStage && plantedSeedCount > 0 && spawnedSecondStage < plantedSeedCount)
        {

            spawnedSecondStage = plantedSeedCount;
            for (int i = 0; i < 3; i++)
            {
                int randomPosition = Random.Range(0, 3);
                if (!positionInts.Contains(randomPosition))
                {
                    positionInts.Add(randomPosition);
                }
                else
                {
                    i--;
                }

            }
            StartCoroutine(PlantSeedSecondStage(0.15f));
            foreach (var obj in currentSeeds)
            {
                if (obj != null)
                    Destroy(obj);
            }
            currentSeeds.Clear();
            // Debug.Log("Second stage placed");
        }

        if (thirdStage && plantedSeedCount > 0 && spawnedThirdStage < plantedSeedCount)
        {
            positionInts.Clear();
            for (int i = 0; i < 3; i++)
            {
                int randomPosition = Random.Range(0, 3);
                if (!positionInts.Contains(randomPosition))
                {
                    positionInts.Add(randomPosition);
                }
                else
                {
                    i--;
                }

            }
            spawnedThirdStage = plantedSeedCount;
            foreach (var obj in currentSecondStage)
            {
                if (obj != null)
                    Destroy(obj);
            }
            currentSecondStage.Clear();
            StartCoroutine(PlantSeedThirdStage(0.15f));

            // Debug.Log("Third stage placed");
        }
    }
    public IEnumerator PlantSeedFirstStage(float timer)
    {
        for (int i = 0; i < 3; i++)
        {

            //Vector3 spawnPos = new Vector3(transform.position.x , transform.position.y , transform.position.z );
            GameObject instance = Instantiate(seed, seedParents[i].transform.position, transform.rotation, seedParents[i].transform);


            plantedSeedCount++;
            nextSeedIndex++;
            currentSeeds.Add(instance);
            yield return new WaitForSeconds(timer);

        }
    }
    public IEnumerator PlantSeedSecondStage(float timer)
    {
        for (int i = 0; i < 3; i++)
        {

            int randomFlower = Random.Range(0, 3);
            flowerIndexes.Add(randomFlower);
            selectedPrimary = fireSecondStage[randomFlower];
            GameObject instance = Instantiate(selectedPrimary, seedParents[positionInts[i]].transform.position, transform.rotation, seedParents[i].transform);
            spawnedSecondStage++;
            currentSecondStage.Add(instance);
            yield return new WaitForSeconds(timer);
        }
    }
    public IEnumerator PlantSeedThirdStage(float timer)
    {
        for (int i = 0; i < 3; i++)
        {

            selectedSecondary = fireThirdStage[flowerIndexes[i]];
            GameObject instance = Instantiate(selectedSecondary, seedParents[i].transform.position, transform.rotation, seedParents[i].transform);
            spawnedThirdStage++;
            currentThirdStage.Add(instance);
            yield return new WaitForSeconds(timer);
        }
        fireSeeds = 3;

    }
}
