using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnoffFog : MonoBehaviour
{
    

    void Start()
    {
        RenderSettings.fog = true;

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RenderSettings.fog = false;

        }

    }

}
