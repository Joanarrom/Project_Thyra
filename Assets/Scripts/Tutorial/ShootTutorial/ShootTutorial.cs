using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTutorial : MonoBehaviour
{
    public GameObject player; 
    public MonoBehaviour projectileScript;
    public MonoBehaviour playerShootingScript; 
    public Transform activationPoint; 

    private bool activated = false;

    void Start()
    {
        if (projectileScript == null) projectileScript = player.GetComponent<Projectile>();
        if (playerShootingScript == null) playerShootingScript = player.GetComponent<PlayerShooting>();

        if (projectileScript != null) projectileScript.enabled = false;
        if (playerShootingScript != null) playerShootingScript.enabled = false;
    }

    void Update()
    {
        if (!activated && Vector3.Distance(player.transform.position, activationPoint.position) < 5f)
        {
            
            if (projectileScript != null) projectileScript.enabled = true;
            if (playerShootingScript != null) playerShootingScript.enabled = true;
            
            activated = true; 
        }
    }

    bool IsInTutorialScene()
  {
    return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TutorialScene";
  }
}
