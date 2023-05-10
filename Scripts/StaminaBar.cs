using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    [Range(0, 4000)]

    public int stamina;
    public int maxStamina = 2000;

    public RectTransform uiBar;

    float percentUnit;
    float staminaPercentUnit;

    private void Start()
    {
        percentUnit = 1f / uiBar.anchorMax.x;
        staminaPercentUnit = 100f / maxStamina;
    }

    private void Update()
    {
        if (stamina > maxStamina)
            stamina = maxStamina;
        else if (stamina < 0)
            stamina = 0;

        float currentStaminaPercent = stamina * staminaPercentUnit;

        uiBar.anchorMax = new Vector2((currentStaminaPercent * percentUnit) / 100f, uiBar.anchorMax.y);
    }

    private void OnValidate()
    {
        if (stamina > maxStamina)
            stamina = maxStamina;
        else if (stamina < 0)
            stamina = 0;
    }
}
