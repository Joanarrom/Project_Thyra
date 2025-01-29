using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
     public static GameManager Instance;  
    public bool dashEnabled = false;    

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    private void Start()
    {
        
     dashEnabled = false;
     Debug.Log("Estado de Dash al inicio del juego: " + dashEnabled);
    }

    public void SaveDashState()
    {
        
     if (dashEnabled)
     {
        PlayerPrefs.SetInt("DashEnabled", 1);
     }
     else
     {
        PlayerPrefs.SetInt("DashEnabled", 0);
     }
     PlayerPrefs.Save();
    }

    public void UnlockDash()
    {
        
        dashEnabled = true;
        SaveDashState(); 
        Debug.Log("Dash Habilitado");
    }
}
