using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{ 
      public float effectDuration = 0.5f;  
    public GameObject redScreenEffect;  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
           
            TestEnergy playerScript = other.GetComponent<TestEnergy>();

           
            if (playerScript != null && !playerScript.isDashing)
            {
                
                StartCoroutine(RedScreenEffect());
            }
        }
    }

    private IEnumerator RedScreenEffect()
    {
        
        redScreenEffect.SetActive(true);

        
        yield return new WaitForSeconds(effectDuration);

       
        redScreenEffect.SetActive(false);
    }
    

}
