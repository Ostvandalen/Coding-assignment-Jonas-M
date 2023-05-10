using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Teleport script. Didnt get it to work as intended

    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            player.transform.position = respawnPoint.transform.position;
        Debug.Log("teleport");
    }



}