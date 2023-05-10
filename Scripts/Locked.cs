using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : Interactable
{
    public bool start = false;
    private bool isOpen = false;
    private bool canBeInteractedWith = true;
    private Animator anim;

    public GameObject trigger;


    private void Start()
    {
        anim = GetComponent<Animator>();
        trigger.SetActive(false);
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
            FindObjectOfType<AudioManager>().Play("Locked");
            trigger.SetActive(true);
            Debug.Log("Door is Locked");
            if (trigger.activeSelf)
            {
                new WaitForSeconds(3);
                //FindObjectOfType<AudioManager>().Play("Locked");
                trigger.SetActive(false);
            }
            // StartCoroutine(AutoClose());

        }
    }
    public override void OnLooseFocus()
    {

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
