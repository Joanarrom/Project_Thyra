using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class PostApi : MonoBehaviour
{
    public TMP_InputField nombreInput;  
    public TextMeshProUGUI puntuacionText; 
    public Button enviarButton; 
    public EnviarPuntuacion enviador; 

    private void Start()
    {
        
        int score = ScoreManager.Instance.GetScore();
        
        puntuacionText.text = "Tu puntuación: " + score;

       
        enviarButton.onClick.AddListener(EnviarPuntuacionFinal);
    }

    public void EnviarPuntuacionFinal()
    {
        
        string nombre = nombreInput.text.Trim();
       
        int score = ScoreManager.Instance.GetScore();

        
        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Introduce un nombre antes de enviar.");
            return;
        }

        
        enviarButton.interactable = false;

       
        enviador.Enviar(nombre, score);
        Debug.Log("Enviando puntuación: " + score);

        
        ScoreManager.Instance.ResetScore();
        Debug.Log("Puntuación reseteada.");
    }
}
