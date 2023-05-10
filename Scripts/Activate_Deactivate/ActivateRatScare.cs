using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRatScare : MonoBehaviour
{
    public GameObject trigger;
    

    void Start()
    {
        trigger.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Activate"))
        {

            trigger.SetActive(true);
            

        }

    }
}
