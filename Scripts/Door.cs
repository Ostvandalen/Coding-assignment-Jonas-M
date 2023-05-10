using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    // Scrip for doors. Animation needs to be made for "open" and "close" then set up the animation with bool "isOpen". Add script to door and make sure the door isnt set to static
    public bool start = false;
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


  public override void OnFocus()
    {

    }

  public override void OnInteract()
    {
        if (canBeInteractedWith)
        {
            isOpen = !isOpen;

            Vector3 doorTransformDirection = transform.TransformDirection(Vector3.forward);
            Vector3 playerTransformDirection = PlayerController.instance.transform.position - transform.position;
            float dot = Vector3.Dot(doorTransformDirection, playerTransformDirection);

            anim.SetFloat("dot", dot);
            anim.SetBool("isOpen", isOpen);
            FindObjectOfType<AudioManager>().Play("DoorOpen");

            StartCoroutine(AutoClose());

        }
    }
   public override void OnLooseFocus()
    {

    }

    private IEnumerator AutoClose()
    {
        while (isOpen)
        {
            yield return new WaitForSeconds(3); // seconds before the door autocloses

            if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 3) //checks if the player is 3 units away from the door
            {
                
                isOpen = false;
                anim.SetFloat("dot", 0); // makes object uninteractable during the animation (needs to be set up in the animation)
                anim.SetBool("isOpen", isOpen);
                yield return new WaitForSeconds(1); // waits 1 second so the sound syncs with the animation
                FindObjectOfType<AudioManager>().Play("DoorClose");
            }
        }
    }

    private void Animator_LockInteraction()
    {
        canBeInteractedWith = false;
    }

    private void Animator_UnlockInteraction()
    {
        canBeInteractedWith = true;
    }
}
