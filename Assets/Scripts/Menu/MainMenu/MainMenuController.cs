using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
   
    public void StartGame()
    {
        SceneManager.LoadScene("Boss_Scene"); 

    }
  
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();  
    }
}
