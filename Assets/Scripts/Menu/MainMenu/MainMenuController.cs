using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
   
    public void StartGame()
    {
        SceneManager.LoadScene("BossLevel"); 

    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene");  

    }
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();  
    }
}
