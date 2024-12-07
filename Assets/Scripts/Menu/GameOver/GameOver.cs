using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        // Asegúrate de que el cursor sea visible y no esté bloqueado
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Spawn()
    {
        SceneManager.LoadScene("BossLevel"); // Vuelve al nivel del jefe
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Regresa al menú principal
    }
}