using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
   private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.OnTakeDamage(0.3f);
            //FindObjectOfType<AudioManager>().Play("Hurt");
        }
            
            

    }
}
