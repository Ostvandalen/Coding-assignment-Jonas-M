using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    public GameObject trigger;

    void Start()
    {
        trigger.SetActive(true);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Activate"))
        {
            trigger.SetActive(false);

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Activate"))
        {
            trigger.SetActive(true);

        }
    }
}
