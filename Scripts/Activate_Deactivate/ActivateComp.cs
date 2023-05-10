using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateComp : MonoBehaviour
{


    public GameObject trigger;
    

    void Start()
    {
        trigger.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            trigger.SetActive(true);
            

        }

    }

  
}
