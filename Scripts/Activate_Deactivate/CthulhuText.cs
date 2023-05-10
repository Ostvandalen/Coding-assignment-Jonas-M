using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CthulhuText : MonoBehaviour
{

    

    public GameObject trigger;
    

    void Start()
    {
        trigger.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Cthulhu"))
        {

            trigger.SetActive(true);
            

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cthulhu"))
        {
            trigger.SetActive(false);

        }
    }
}
