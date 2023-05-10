using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastScareSound : MonoBehaviour
{
    // Plays sound when entering the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            FindObjectOfType<AudioManager>().Play("AAA");
            Debug.Log("HurtSound");
        }



    }
}
