using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtPlayer : MonoBehaviour
{
    public Transform player; 
    public float maxDistance = 10f; 

    private Canvas canvas; 

    void Start()
    {
        canvas = GetComponent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("El canvas no está asignado o no tiene el componente Canvas.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        
        canvas.enabled = distance <= maxDistance;

       
        if (canvas.enabled)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; 
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
