using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEactivateComp : MonoBehaviour
{
    public GameObject trigger;

    void Start()
    {
        trigger.SetActive(true);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            trigger.SetActive(false);

        }

    }
}
