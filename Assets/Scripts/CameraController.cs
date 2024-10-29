using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation += mouseX;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
    }
}
