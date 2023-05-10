using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerController.OnTakeDamage(50);
    }
}

