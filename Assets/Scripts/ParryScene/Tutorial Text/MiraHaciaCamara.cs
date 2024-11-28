using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraHaciaCamara : MonoBehaviour
{
   
    public Camera camera;

    void Update()
    {
        Vector3 direccion = camera.transform.position - transform.position;
        Quaternion rotacion = Quaternion.LookRotation(direccion);
        transform.rotation = rotacion;
    }

}
