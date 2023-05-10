using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : Interactable
{

    // just a test script to see if "interactable" works as it should


    public override void OnFocus()
    {
        print("Looking at" + gameObject.name);
    }

    public override void OnInteract()
    {
        print("Interacted with" + gameObject.name);
    }

    public override void OnLooseFocus()
    {
        print("Stopped looking at" + gameObject.name);
    }
}
