using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; 
    public Transform[] spawnPoints; 
    public Transform player; 
    public float spawnDelay = 1f; 

    private int currentIndex = 0; 

    void Start()
    {
        
        StartCoroutine(SpawnArrows());
    }

    IEnumerator SpawnArrows()
    {
        
        while (currentIndex < spawnPoints.Length)
        {
            
            Transform point = spawnPoints[currentIndex];

            
            GameObject arrow = Instantiate(arrowPrefab, point.position, Quaternion.identity);

            
            arrow.GetComponent<ArrowIndicator>().player = player;

           
            yield return new WaitUntil(() => arrow == null); 

            
            currentIndex++;
            
           
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
