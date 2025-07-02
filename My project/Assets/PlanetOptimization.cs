using UnityEngine;

public class PlanetOptimization : MonoBehaviour
{
    public Transform planet;

    public Vector2 fireAreaEnabledRange;
    public Vector2 waterAreaEnabledRange;
    public Vector2 crystalAreaEnabledRange;

    public GameObject firePlain;
    public GameObject waterPlain;
    public GameObject crystalPlain;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (planet.transform.rotation.z >= waterAreaEnabledRange[1] && planet.transform.rotation.z <= waterAreaEnabledRange[2])
        {
            firePlain.SetActive(false);
            crystalPlain.SetActive(false);
            waterPlain.SetActive(true);
        }
        else if (planet.transform.rotation.z >= fireAreaEnabledRange[1] && planet.transform.rotation.z <= fireAreaEnabledRange[2])
        {
            firePlain.SetActive(true);
            crystalPlain.SetActive(false);
            waterPlain.SetActive(false);
        }
        else
        {
            firePlain.SetActive(false);
            crystalPlain.SetActive(true);
            waterPlain.SetActive(false);
        }

    }
}
