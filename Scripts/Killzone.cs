using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    

    void OnTriggerEnter(Collider player)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        
        Debug.Log("Died");
    }
}
