using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInteraction : MonoBehaviour
{
   
     public Camera mainCamera; // Cámara principal
    public Camera puzzleCamera; // Cámara del puzzle
    public GameObject player; // El personaje del jugador
    public GameObject lasers; // Los láseres a desactivar
    public GameObject puzzle; // El laberinto y la bola
    public Transform ballStartPosition; // Posición inicial de la bola
    public GameObject ball; // Referencia a la bola
    private LabyrinthPuzzle labyrinthPuzzle; // Referencia al script del puzzle
    private bool isPuzzleActive = false;

    private void Start()
    {
        labyrinthPuzzle = puzzle.GetComponent<LabyrinthPuzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPuzzleActive)
        {
            Debug.Log("Presiona 'E' para interactuar con el panel.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isPuzzleActive)
        {
            ActivatePuzzle();
        }
    }

    public void ActivatePuzzle()
    {
        isPuzzleActive = true;

        // Cambiar a la cámara del puzzle
        mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);

        // Desactivar controles del jugador
        player.GetComponent<TestEnergy>().enabled = false;

        // Resetear la posición de la bola
        ResetBallPosition();

        // Activar el puzzle
        puzzle.SetActive(true);

        // Habilitar controles de la bola en el script del puzzle
        if (labyrinthPuzzle != null)
        {
            labyrinthPuzzle.isPuzzleActive = true;
        }
    }

    public void DeactivatePuzzle()
    {
        isPuzzleActive = false;

        // Cambiar a la cámara principal
        puzzleCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        // Reactivar controles del jugador
        player.GetComponent<TestEnergy>().enabled = true;

        // Desactivar el puzzle
        puzzle.SetActive(false);

        // Apagar los láseres
        lasers.SetActive(false);

        // Deshabilitar controles de la bola
        if (labyrinthPuzzle != null)
        {
            labyrinthPuzzle.isPuzzleActive = false;
        }
    }

    private void ResetBallPosition()
    {
        ball.transform.position = ballStartPosition.position;
        ball.transform.rotation = ballStartPosition.rotation;

        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }
    }
}
