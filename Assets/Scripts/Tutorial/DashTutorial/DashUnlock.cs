using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUnlock : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            GameManager.Instance.dashEnabled = true;
            GameManager.Instance.SaveDashState();  

            Debug.Log("Dash Habilitado");

            
            Destroy(gameObject);
        }
    }
}
