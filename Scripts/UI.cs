using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    // Script for the health and stamina UI. UI needs to be set up in Unity for it to work.


    [SerializeField] private TextMeshProUGUI healthtext = default;
    [SerializeField] private TextMeshProUGUI staminatext = default;
   
    


    private void OnEnable()
    {
        PlayerController.OnDamage += UpdateHealth;
        PlayerController.OnHeal += UpdateHealth;
        PlayerController.OnStaminaChange += UpdateStamina;
    }

    private void OnDisable()
    {
        PlayerController.OnDamage -= UpdateHealth;
        PlayerController.OnHeal -= UpdateHealth;
        PlayerController.OnStaminaChange -= UpdateStamina;
    }

    private void Start()
    {
        UpdateHealth(100); // applies the health when starting
        UpdateStamina(100); // applies the stamina when starting
        
    }

   private void UpdateHealth(float currentHealth)
    {
       healthtext.text = currentHealth.ToString("00");
      
    }

    private void UpdateStamina(float currentStamina)
    {
        staminatext.text = currentStamina.ToString("00");
       
    }

  




}
