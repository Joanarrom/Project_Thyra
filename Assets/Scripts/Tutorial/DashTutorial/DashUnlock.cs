using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUnlock : MonoBehaviour
{
    public TestEnergy playerScript; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerScript.dashEnabled = true;
            Debug.Log("Dash Habilitado");
            Destroy(gameObject);
        }
    }
}
