using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ForceUIClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left-click
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, hits);

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.GetComponent<Button>())
                {
                    ExecuteEvents.Execute(hit.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                    Debug.Log($"🖱️ Clicked UI button: {hit.gameObject.name}");
                    break; // stop after first clickable
                }
            }
        }
    }
}
