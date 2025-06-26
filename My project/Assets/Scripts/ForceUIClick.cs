using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class ForceUIClick : MonoBehaviour
{
    void Update()
    {
        //check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            //create a pointer event at mouse position
            PointerEventData pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            //raycast to detect UI elements under the cursor
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, hits);

            //loop through the hit results
            foreach (RaycastResult hit in hits)
            {
                //if a Button component is found trigger its OnClick event using ExecuteEvents
                if (hit.gameObject.GetComponent<Button>())
                {
                    ExecuteEvents.Execute(hit.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                    break; //stop after triggering the click
                }
            }
        }
    }

}
