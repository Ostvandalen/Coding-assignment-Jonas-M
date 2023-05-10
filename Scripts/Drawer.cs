using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Interactable
{
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
            FindObjectOfType<AudioManager>().Play("Drawer");

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

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 3) //checks if the player is 3 units away from the door
            {
                
                isOpen = false;
                anim.SetFloat("dot", 0); // Makes object uninteractable during animation (needs to be set up in the animation)
                anim.SetBool("isOpen", isOpen);
                yield return new WaitForSeconds(1);  // waits 1 second before playing sfx to sync with animation, just change the amount if it needs to be more exact
                FindObjectOfType<AudioManager>().Play("DrawerClose");
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
