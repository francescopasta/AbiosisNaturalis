using UnityEngine;
using UnityEngine.UI;

public class ShopDropdown : MonoBehaviour
{
    public GameObject panel;
    public GameObject manager;

    private void Start()
    {
        Toggle myToggle = GetComponent<Toggle>();

        if (myToggle != null)
        {
            myToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    public void OnToggleValueChanged(bool isOn)
    {
        panel.SetActive(isOn);

        if (manager != null)
        {
            PlanetMovement planet = manager.GetComponent<PlanetMovement>();
            if (planet != null)
            {
                planet.enabled = !isOn; // Disable movement when panel is open
            }
        }
    }
}
