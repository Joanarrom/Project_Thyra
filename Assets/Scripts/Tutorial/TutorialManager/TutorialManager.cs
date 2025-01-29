using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialCanvases; 
    public Transform player;
    public Transform[] tutorialZones;
    public float activationDistance = 5f; 

    private int currentTutorialIndex = -1; 

    void Update()
    {
        CheckPlayerZone();
    }

    void CheckPlayerZone()
    {
        for (int i = 0; i < tutorialZones.Length; i++)
        {
            float distance = Vector3.Distance(player.position, tutorialZones[i].position);

            if (distance <= activationDistance)
            {
                ShowTutorial(i);
                break;
            }
        }
    }

    void ShowTutorial(int index)
    {
        if (index == currentTutorialIndex) return; 

       
        if (currentTutorialIndex >= 0)
            tutorialCanvases[currentTutorialIndex].SetActive(false);

       
        tutorialCanvases[index].SetActive(true);
        currentTutorialIndex = index;
    }
}
