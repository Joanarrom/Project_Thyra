using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurbineFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
{
    // Verifica que el objeto que entra sea el jugador
    if (other.CompareTag("Player"))
    {
         SceneManager.LoadScene("GameOver");
    }
}
}
