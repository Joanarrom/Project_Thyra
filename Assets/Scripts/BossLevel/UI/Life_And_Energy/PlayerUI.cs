using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider; 
    public Slider energySlider; 

    //  barras de vida y energía
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

    // Actualizar la barra de vida
    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    // Actualizar la barra de energía
    public void UpdateEnergy(int currentEnergy)
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }
    }
}
