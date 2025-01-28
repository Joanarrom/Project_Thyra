using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; 
    public Transform[] spawnPoints; 
    public Transform player; 

    void Start()
    {
        
        foreach (Transform point in spawnPoints)
        {
            GameObject arrow = Instantiate(arrowPrefab, point.position, Quaternion.identity);
            arrow.GetComponent<ArrowIndicator>().player = player;
        }
    }
}
