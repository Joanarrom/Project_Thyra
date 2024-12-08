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

   
    public void OpenControls()
    {
        SceneManager.LoadScene("ControlsScene");  
    }

    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
