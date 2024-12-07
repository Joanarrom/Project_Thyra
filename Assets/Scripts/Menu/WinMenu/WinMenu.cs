using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinMenu : MonoBehaviour
{
    void Start()
    {
       
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

   public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
