using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjective : MonoBehaviour
{
    public static bool showObjective = false;

    public GameObject objective;
    public GameObject healthUI;

    // disable and enable the ingame UI

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (showObjective)
            {

                Close();

            }
            else
            {
                Show();

            }
        }

    }

    public void Close()
    {
        Debug.Log("closed");
        objective.SetActive(false);
        healthUI.SetActive(false);
        showObjective = false;
        

    }
    void Show()
    {
        objective.SetActive(true);
        healthUI.SetActive(true);
        showObjective = true;
        
    }
}
