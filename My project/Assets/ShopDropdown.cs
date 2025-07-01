using UnityEngine;

using UnityEngine.UI;
public class ShopDropdown : MonoBehaviour
{
    public GameObject panel;
    public GameObject manager;
    private void Start()
    {
        Button myButton = GetComponent<Button>();

        if (myButton != null)
        {
            myButton.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        panel.SetActive(!panel.activeSelf);
        PlanetMovement planet = manager.GetComponent<PlanetMovement>();
        if (manager != null) 
        {
            planet.enabled = !planet.isActiveAndEnabled;
        }
    }
}
