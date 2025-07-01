using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class ShopManagementUpdated : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that will manage all the shop.   " +
        "2. Do the same for the objects with the _WaterFlowe / FireFlower / CrystalFlower_ scripts.  " +
        "3. Place the assets for the upgrade under _UpgradeAssets_.   " +
        "4. Do the same with the object with the _WaterMeter_ script.   " +
        "5. Change the amound of currency that will be given to the player for the water, fire and crystal flowers under _CurrencyPerWater/Fire/Crystal_.   " +
        "5. Change the price of the upgrades under _PricePerUpgrade_.  " +
        "6. Under _Upgrades_, add 3 values. All of them should represent empty boxes.   " +
        "7. For more information, hover your mouse over the variables." +
        "8. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    [Tooltip("The total amound of currency the player has.")]
    public int totalCurrency;
    [Tooltip("The amound of currency the player will gain for a water flower.")]
    public int currencyPerWater;
    [Tooltip("The amound of currency the player will gain for a fire flower.")]
    public int currencyPerFire;
    [Tooltip("The amound of currency the player will gain for a crystal flower.")]
    public int currencyPerCrystal;

    [Tooltip("The price of a given upgrade")]
    public int[] pricePerUpgrade;
    public int[] pricePerGarden;

    [Tooltip("Booleans to keep track of each upgrade is activated.")]
    public bool[] upgades;

    public bool[] gardenUnlock;

    //UI
    public GameObject firePanelUI;
    public GameObject crystalPanelUI;
    public TextMeshProUGUI waterIncome;
    public TextMeshProUGUI fireIncome;
    public TextMeshProUGUI crystalIncome;
    public TextMeshProUGUI totalAmount;
    public FlowerManagerUpdate flowerManager;
    public WaterMeterUpdated waterMeterUpdated;

    public void Update()
    {
        waterIncome.text = currencyPerWater.ToString() + "per Flower";
        fireIncome.text = currencyPerFire.ToString() + "per Flower";
        crystalIncome.text = currencyPerCrystal.ToString() + "per Flower";
        totalAmount.text = totalCurrency.ToString(); 
    }

    public void ShoppingButtonNewGarden()
    {
        if (totalCurrency >= pricePerGarden[0] && !gardenUnlock[0])
        {
            gardenUnlock[0] = true; //fire garden
            firePanelUI.SetActive(false);
            waterMeterUpdated.firePlantUnlocked = true;
            totalCurrency -= pricePerGarden[0];
        }

        else if (totalCurrency >= pricePerGarden[1] && !gardenUnlock[1] && gardenUnlock[0])
        {
            gardenUnlock[1] = true; //crystal garden
            crystalPanelUI.SetActive(false);
            waterMeterUpdated.crystalPlantUnlocked = true;
            totalCurrency -= pricePerGarden[1];
        }
    }

    public void ShoppingButtonUpgrade()
    {
        if (totalCurrency >= pricePerUpgrade[0] && !upgades[0])
        {
            upgades[0] = true; //water garden
            totalCurrency -= pricePerUpgrade[0];
            flowerManager.AutomateGarden(0);
            
        }

        else if (totalCurrency >= pricePerUpgrade[1] && !upgades[1])
        {
            upgades[1] = true;  //fire garden
            totalCurrency -= pricePerUpgrade[1];
            flowerManager.AutomateGarden(1);
        }

        else if (totalCurrency >= pricePerUpgrade[2] && !upgades[2])
        {
            upgades[2] = true; //crystal garden
            totalCurrency -= pricePerUpgrade[2];
            flowerManager.AutomateGarden(2);
        }
    }
}
