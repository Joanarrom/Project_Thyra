using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerSource : MonoBehaviour
{
    public static PowerSourceManager Instance; // Asegúrate de que esta sea la instancia de PowerSourceManager
    public int health = 100;  // Salud del PowerSource

    public event Action OnAllPowerSourcesDestroyed; // Evento para notificar cuando todos los PowerSources sean destruidos

    void Awake()
    {
        // Si PowerSourceManager aún no tiene instancia, la asignamos.
        if (Instance == null)
        {
            Instance = FindObjectOfType<PowerSourceManager>(); // Obtener el PowerSourceManager que maneja la lógica
        }

        if (Instance == null)
        {
            Debug.LogError("PowerSourceManager no encontrado en la escena.");
        }
    }

    void Start()
    {
        // Registrar este PowerSource en PowerSourceManager.
        Instance.RegisterPowerSource(this);
    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
                  
         EnemyScore score = GetComponent<EnemyScore>();
         if (score != null)
         {
            score.Morir(); // Esto suma puntos y destruye el objeto
         }
         else
         {
            Destroy(gameObject); // Fallback por si no tiene EnemyScore
         }

         
         Instance.PowerSourceDestroyed();
        }
    }
}
