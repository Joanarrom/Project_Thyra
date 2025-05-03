using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{public Transform player;
    public Transform cameraPivot;
    public float mouseSensitivity = 100f;
    public float cameraDistance = 5f;
    public Vector2 verticalClamp = new Vector2(-30, 60);
    
    private float mouseX;
    private float mouseY;
    private float currentCameraDistance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentCameraDistance = cameraDistance;
        
        // Asegurar que el pivot empiece en la posición correcta
        cameraPivot.position = player.position;
    }

    void LateUpdate()
    {
        RotateWithMouse();
        FollowPlayer();
        HandleCameraCollision();
    }

    void RotateWithMouse()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, verticalClamp.x, verticalClamp.y);

        // Rotación independiente del pivot en ambos ejes
        cameraPivot.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }

    void FollowPlayer()
    {
        // Mantener el pivot en la posición del jugador
        cameraPivot.position = player.position;
    }

    void HandleCameraCollision()
    {
        RaycastHit hit;
        Vector3 desiredPosition = cameraPivot.position - cameraPivot.forward * cameraDistance;
        
        if(Physics.Linecast(cameraPivot.position, desiredPosition, out hit))
        {
            currentCameraDistance = Mathf.Clamp(hit.distance, 1f, cameraDistance);
        }
        else
        {
            currentCameraDistance = cameraDistance;
        }
        
        transform.position = cameraPivot.position - cameraPivot.forward * currentCameraDistance;
        transform.LookAt(cameraPivot.position);
    }
}
