using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activate : MonoBehaviour
{

    [SerializeField] private UnityEvent shakeEvent;

    public GameObject trigger;
    private ShakeEffect shakeEffect;

    void Start()
    {
        trigger.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Activate"))
        {
            
            trigger.SetActive(true);
            shakeEvent.Invoke();

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Activate"))
        {
            trigger.SetActive(false);

        }
    }
}
