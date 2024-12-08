using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableSystem : MonoBehaviour
{
    public Image healthConsumableIcon;
    public Image energyConsumableIcon;
    public Text healthConsumableCountText;
    public Text energyConsumableCountText;

    public int healthConsumableCount = 3;
    public int energyConsumableCount = 3;

    public KeyCode useHealthKey = KeyCode.Alpha1;
    public KeyCode useEnergyKey = KeyCode.Alpha2;

    private TestEnergy player;

    private void Start()
    {
        player = FindObjectOfType<TestEnergy>();
        UpdateUI();
    }

    private void Update() //Comprueba si el jugador presiona la tecla asignada para uno de los consumibles
    {
        if (Input.GetKeyDown(useHealthKey) && healthConsumableCount > 0)
        {
            UseHealthConsumable();
        }

        if (Input.GetKeyDown(useEnergyKey) && energyConsumableCount > 0)
        {
            UseEnergyConsumable();
        }
    }

    private void UseHealthConsumable() //Consume un ítem de salud y restaura vida al jugador.
    {
        if (healthConsumableCount > 0)
        {
            healthConsumableCount--;
            player.RestoreHealth(20);
            player.playerUI?.UpdateHealth(player.health);
            UpdateUI();
        }
    }

    private void UseEnergyConsumable() //Consume un ítem de energía y restaura energía al jugador.
    {
        if (energyConsumableCount > 0)
        {
            energyConsumableCount--;
            player.RestoreEnergy(30);
            player.playerUI?.UpdateEnergy(player.energy);
            UpdateUI();
        }
    }

    private void UpdateUI() //Actualiza la cantidad de items disponibles en la UI.
    {
        healthConsumableCountText.text = healthConsumableCount.ToString();
        energyConsumableCountText.text = energyConsumableCount.ToString();
    }
}
