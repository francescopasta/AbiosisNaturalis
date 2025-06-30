using UnityEngine;

using UnityEngine.UI;
public class ShopDropdown : MonoBehaviour
{
    public GameObject panel;
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
    }
}
