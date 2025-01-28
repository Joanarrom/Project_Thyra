using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthPuzzle : MonoBehaviour
{
    public Transform ball; // La bola en el laberinto
    public Transform target; // El punto objetivo
    public float speed = 5f; // Velocidad de movimiento
    public bool isPuzzleActive = false; // Control del estado del puzzle

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
            return; // Evitar movimiento si el puzzle no está activo
        }

        // Controlar la bola con las teclas de flechas o WASD
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        ball.Translate(moveX, 0, moveZ);

        // Verificar si la bola alcanza el punto objetivo
        if (Vector3.Distance(ball.position, target.position) < 0.5f)
        {
            Debug.Log("Puzzle resuelto!");
            panelInteraction.DeactivatePuzzle();
        }
    }
}
