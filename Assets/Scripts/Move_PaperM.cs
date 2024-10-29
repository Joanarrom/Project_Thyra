using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_PaperM : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float distanceToMove = 4f;
    public float rotationSpeed = 5f; 

    private Vector3 startingPosition;
    private bool movingRight = true;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        
        Vector3 targetPosition = transform.position;
        if (movingRight)
        {
            targetPosition.x = startingPosition.x + distanceToMove;
        }
        else
        {
            targetPosition.x = startingPosition.x - distanceToMove;
        }

        
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(movingRight ? Vector3.left : Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                movingRight = !movingRight;
            }
        }
        else
        {
           
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }
}