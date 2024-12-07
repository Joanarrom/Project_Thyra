using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class MenuOptions : MonoBehaviour
{
    public void OpenMusic()
    {
      SceneManager.LoadScene("");
    }   

    public void OpenVideo()
    {
        SceneManager.LoadScene("");  
    }

    // Método para abrir el menú de configuración (Settings)
    public void OpenControls()
    {
        SceneManager.LoadScene("ControlsScene");  
    }

    // Método para salir del juego
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
