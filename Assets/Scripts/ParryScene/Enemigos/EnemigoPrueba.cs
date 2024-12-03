using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoPrueba : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 10f;
    public int damageAmount = 20; // Daño que inflige al jugador en cada colisión

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimiento de ping-pong entre la posición inicial y la distancia especificada
        float newPosition = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPosition + Vector3.right * newPosition;
    }

    // Detectar colisión con el jugador
    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisionamos es el jugador
       TestEnergy player = collision.gameObject.GetComponent<TestEnergy>();
        
        if (player != null)
        {
            // Si colisiona con el jugador, aplicamos daño
            player.TakeDamage(damageAmount);
            Debug.Log("El enemigo infligió " + damageAmount + " de daño al jugador.");
        }
    }
}

