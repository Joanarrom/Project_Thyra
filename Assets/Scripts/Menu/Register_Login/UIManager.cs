using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject registerPanel;
    public GameObject loginPanel;
    public GameObject selectionMenuCanvas;

    public void ShowRegisterPanel()
    {
        TogglePanels(registerPanel);
    }

    public void ShowLoginPanel()
    {
        TogglePanels(loginPanel);
    }

    private void TogglePanels(GameObject activePanel)
    {
        
        registerPanel.SetActive(false);
        loginPanel.SetActive(false);
        selectionMenuCanvas.SetActive(false);
        
        
        activePanel.SetActive(true);
    }

    public void BackToSelectionMenu()
    {
       
        registerPanel.SetActive(false);
        loginPanel.SetActive(false);
        selectionMenuCanvas.SetActive(true);
    }
}
