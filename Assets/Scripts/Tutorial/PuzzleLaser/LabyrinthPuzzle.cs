using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthPuzzle : MonoBehaviour
{
    public Transform ball;
    public Transform target; 
    public float speed = 5f; 
    public bool isPuzzleActive = false; 

    private PanelInteraction panelInteraction;

    private void Start()
    {
        panelInteraction = FindObjectOfType<PanelInteraction>();
    }

    private void Update()
    {
        if (!isPuzzleActive)
        {
            Debug.Log("El puzzle no está activo, no se permite movimiento.");
            return; 
        }

       
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        ball.Translate(moveX, 0, moveZ);

       
        if (Vector3.Distance(ball.position, target.position) < 0.5f)
        {
            Debug.Log("Puzzle resuelto!");
            panelInteraction.DeactivatePuzzle();
        }
    }
}
