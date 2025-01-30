using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
   public Transform door; 
    public Vector3 openPositionOffset; 
    public float speed = 2f;
    public float delayToClose = 1f; 

    private Vector3 closedPosition; 
    private Vector3 openPosition; 
    private Coroutine doorCoroutine; 
    private bool isPlayerDetected = false; 

    private void Start()
    {
        if (door == null)
        {
            Debug.LogError("Debes asignar el Transform de la puerta en el inspector.");
            return;
        }

        
        closedPosition = door.position;
        openPosition = closedPosition + openPositionOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerDetected = true;
            if (doorCoroutine != null) StopCoroutine(doorCoroutine); 
            doorCoroutine = StartCoroutine(MoveDoor(openPosition)); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerDetected = false;
            StartCoroutine(CloseDoorWithDelay());
        }
    }

    private IEnumerator CloseDoorWithDelay()
    {
        yield return new WaitForSeconds(delayToClose);
        if (!isPlayerDetected) 
        {
            if (doorCoroutine != null) StopCoroutine(doorCoroutine); 
            doorCoroutine = StartCoroutine(MoveDoor(closedPosition)); 
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, targetPosition, Time.deltaTime * speed); 
            yield return null;
        }
        door.position = targetPosition; 
    }
}
