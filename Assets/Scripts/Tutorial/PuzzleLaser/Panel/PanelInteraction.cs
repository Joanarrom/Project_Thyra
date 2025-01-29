using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInteraction : MonoBehaviour
{
   
     public Camera mainCamera; 
    public Camera puzzleCamera; 
    public GameObject player; 
    public GameObject lasers; 
    public GameObject puzzle; 
    public Transform ballStartPosition; 
    public GameObject ball; 
    private LabyrinthPuzzle labyrinthPuzzle; 
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

       
        mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);

       
        player.GetComponent<TestEnergy>().enabled = false;

        
        ResetBallPosition();

        
        puzzle.SetActive(true);

        
        if (labyrinthPuzzle != null)
        {
            labyrinthPuzzle.isPuzzleActive = true;
        }
    }

    public void DeactivatePuzzle()
    {
        isPuzzleActive = false;

       
        puzzleCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        
        player.GetComponent<TestEnergy>().enabled = true;

       
        puzzle.SetActive(false);

        
        lasers.SetActive(false);

        
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
