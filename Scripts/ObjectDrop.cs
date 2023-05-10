using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrop : MonoBehaviour
{
    public void OnCollisionEnter(Collision col) // Activates sound when object is dropped on the floor
    {
        if(col.gameObject.tag == "Footsteps/WOOD") // if ghrabbable objects is added in rooms with different floor sfx you must add that tag in the code
        {
            FindObjectOfType<AudioManager>().Play("Drop");
        }
    }
}
