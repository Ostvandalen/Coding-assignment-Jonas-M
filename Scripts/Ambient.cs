using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Ambient : MonoBehaviour
{
    public Slider slider;
    public PostProcessProfile ambient;
    public PostProcessLayer layer;

    AutoExposure exposure;
    // Start is called before the first frame update
    void Start()
    {
        ambient.TryGetSettings(out exposure);
        AdjustAmbient(slider.value);
    }

    public void AdjustAmbient(float value)
    {
        if (value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = 0;
        }
    }
}
