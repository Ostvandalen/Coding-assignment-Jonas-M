using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateRatScare : MonoBehaviour
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
}
