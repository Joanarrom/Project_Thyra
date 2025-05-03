using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerSourceManager : MonoBehaviour
{
    public static PowerSourceManager Instance;

    private int totalPowerSources;
    private int destroyedPowerSources;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicializamos el total de PowerSources.
        totalPowerSources = FindObjectsOfType<PowerSource>().Length;
        destroyedPowerSources = 0;
    }

    // Llamado cuando se destruye un PowerSource
    public void PowerSourceDestroyed()
    {
        destroyedPowerSources++;
        CheckIfAllPowerSourcesAreDestroyed();
    }

    // Verifica si todos los PowerSources han sido destruidos
    void CheckIfAllPowerSourcesAreDestroyed()
    {
        if (destroyedPowerSources >= totalPowerSources)
        {
            OnAllPowerSourcesDestroyed?.Invoke();
        }
    }

    // Registro de PowerSource (puede usarse para contar o manejar eventos)
    public void RegisterPowerSource(PowerSource powerSource)
    {
        // Lógica adicional si necesitas registrar cada PowerSource
    }

    // Evento cuando todos los PowerSources han sido destruidos
    public event Action OnAllPowerSourcesDestroyed;
}
