using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    private LightController interactiveObj;

    [SerializeField] private Image crosshair;

    private void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(transform.position, fwd, out RaycastHit hit, rayLength))
        {
            var raycastObj = hit.collider.gameObject.GetComponent<LightController>();
            if(raycastObj != null) // if there is raycast detection
            {
                interactiveObj = raycastObj;
                CrosshairChange(true);
                Debug.Log("raycast hit");
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
        if (interactiveObj != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                interactiveObj.interactSwitch();
            }
        }
    }

    private void ClearInteraction()
    {
        if(interactiveObj != null)
        {
            CrosshairChange(false);
            interactiveObj = null;
        }
    }

    void CrosshairChange(bool on)
    {
        if (on)
        {
            crosshair.color = Color.white; //change to another color and add to other scripts if this is going to be used
        }
        else
        {
            crosshair.color = Color.white;
        }
    }
}
