using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class FallDetector : MonoBehaviour
{
    // Este método se llama cuando un objeto con un collider entra en el trigger
    void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Aquí llamamos a la lógica de "perder" o reiniciar el juego
            PlayerFall(); 
        }
    }

    // Lógica de "perder" cuando el jugador cae al vacío
    void PlayerFall()
    {
        // Aquí puedes poner lo que pasa cuando el jugador cae al vacío
        // Por ejemplo, podrías reiniciar la escena o mostrar un mensaje
        Debug.Log("¡El jugador ha caído al vacío! Has perdido.");
        SceneManager.LoadScene("ScoreFinal");

        // Opcionalmente, puedes reiniciar la escena o mostrar la pantalla de fin de juego
        // UnityEngine.SceneManagement.SceneManager.LoadScene("NombreDeTuEscena");
        // O puedes usar Time.timeScale = 0 para pausar el juego y mostrar un menú de fin de juego
    }
}
