#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class PlanetRotationSaver : MonoBehaviour
{
    public Transform saveParent; // Optional: where to place the saved views
    public string viewPrefix = "PlanetView_";
    private int viewIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Press 'P' to save
        {
            SaveCurrentRotation();
        }
    }

    void SaveCurrentRotation()
    {
        GameObject view = new GameObject(viewPrefix + viewIndex);
        view.transform.rotation = transform.rotation;
        view.transform.position = Vector3.zero; // keep at origin
        if (saveParent != null)
            view.transform.SetParent(saveParent);

        viewIndex++;

        Debug.Log("Saved planet view: " + view.name, view);
    }
}
#endif
