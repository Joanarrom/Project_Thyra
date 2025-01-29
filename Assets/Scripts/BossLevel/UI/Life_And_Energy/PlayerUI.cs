using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider; 
    public Slider energySlider; 

    
    public void InitializeUI(int maxHealth, int currentHealth, int maxEnergy, int currentEnergy)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;
            energySlider.value = currentEnergy;
        }
    }

    
    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

   
    public void UpdateEnergy(int currentEnergy)
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }
}
