using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;

    [SerializeField] private Image crosshair;


    private ObjectGrabbable objectGrabbable;


    private void Update()
    {

        



            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (objectGrabbable == null) // checks if the player can pick up object if the player aint carrying something it pick up item
                {
                    float pickupDistance = 2f;
                    if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
                    {

                        Debug.Log(raycastHit.transform);
                        if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                        {
                            objectGrabbable.Grab(objectGrabPointTransform);

                        }
                    }
                }
                else // checks if the player is already carrying something if so it drops
                {
                    
                    objectGrabbable.Drop();
                    
                    objectGrabbable = null;
                    
                }
            }
        


    }
    
}
