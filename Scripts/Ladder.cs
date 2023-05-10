using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ladder : MonoBehaviour
{

    // put Ladder script on playercontroller and assign Ladder tag on ladders

    public Transform LadderController;
    bool inside = false;
    public float speedUpDown = 10f; //climb speed
    public PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        inside = false;
    }

    void OnTriggerEnter(Collider col) //checks if player has entered ladder collider. if so the PlayerController deactivates and the LadderController takes over until player leaves Laddercollider
    {
        
        if(col.gameObject.tag == "Ladder")
        {
            
            playerController.enabled = false;
            inside = !inside;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Ladder")
        {
            playerController.enabled = true;
            inside = !inside;
        }
    }

    void Update()
    {
        if(inside == true && Input.GetKey("w"))
        {
            Debug.Log("ladder");
            LadderController.transform.position += Vector3.up / speedUpDown;
        }

        if(inside == true && Input.GetKey("s"))
        {
            LadderController.transform.position += Vector3.down / speedUpDown;
        }
    }
}
