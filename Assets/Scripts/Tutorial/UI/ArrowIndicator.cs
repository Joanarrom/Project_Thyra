using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public float hoverSpeed = 2f; 
    public float hoverAmount = 0.5f; 
    public float rotationSpeed = 50f; 
    public float activationDistance = 3f; 
    public Transform player; 

    private Vector3 initialPosition; 

    void Start()
    {
        
        initialPosition = transform.position;
    }

    void Update()
    {
        
        if (player == null) return;

       
        AnimateArrow();

       
        RotateArrow();

        
        CheckPlayerDistance();
    }

    void AnimateArrow()
    {
        
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }

    void RotateArrow()
    {
        
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void CheckPlayerDistance()
    {
        
        float distance = Vector3.Distance(player.position, transform.position);

        
        if (distance <= activationDistance)
        {
            Destroy(gameObject);
        }
    }
}
