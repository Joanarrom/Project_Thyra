using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActivator : MonoBehaviour
{
   public GameObject tutorialCanvas; // Asigna aquí el Canvas con el TutorialCanvasManager

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && tutorialCanvas != null && !tutorialCanvas.activeSelf)
        {
            tutorialCanvas.SetActive(true);

            var manager = tutorialCanvas.GetComponent<TutorialCanvasManager>();
            if (manager != null)
            {
                manager.MostrarTutorialExternamente();
            }
        }
    }
}
