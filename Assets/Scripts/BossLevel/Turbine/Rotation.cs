using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
   
    [SerializeField] private Vector3 velocidadRotacion = new Vector3(0, 100, 0);

  
    void Update()
    {
        
        transform.Rotate(velocidadRotacion * Time.deltaTime);
    }
}
