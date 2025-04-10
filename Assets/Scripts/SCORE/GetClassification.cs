using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GetClassification : MonoBehaviour
{
     public TextMeshProUGUI clasificacionesText;  
    public string apiUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api";  
    public string apiToken = "UN055LmAO0OohtenSN8h0y4gB40wsRTYVZq1OkEOOpIukOXrxjKjeUV7Ftbq";  

    private void Start()
    {
       
        StartCoroutine(GetTopClasificaciones());
    }

    
    private IEnumerator GetTopClasificaciones()
    {
        
        string url = $"{apiUrl}/classification/{apiToken}";

        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            
            if (request.result == UnityWebRequest.Result.Success)
            {
               
                string jsonResponse = request.downloadHandler.text;
                List<PlayerScore> scores = ParseClasificaciones(jsonResponse);
                
                
                DisplayClasificaciones(scores);
            }
            else
            {
                
                Debug.LogError("Error al obtener las clasificaciones: " + request.error);
            }
        }
    }

    
    private List<PlayerScore> ParseClasificaciones(string jsonResponse)
    {
        
        ClasificacionResponse response = JsonUtility.FromJson<ClasificacionResponse>(jsonResponse);
        return response.data;
    }

    
    private void DisplayClasificaciones(List<PlayerScore> scores)
    {
        
        clasificacionesText.text = "Top 5 Clasificaciones:\n";

        
        for (int i = 0; i < Mathf.Min(5, scores.Count); i++)
        {
            clasificacionesText.text += $"{i + 1}. {scores[i].name} - {scores[i].puntuacion}\n";
        }
    }

    
    [System.Serializable]
    public class PlayerScore
    {
        public string name;
        public int puntuacion;
    }

    [System.Serializable]
    public class ClasificacionResponse
    {
        public List<PlayerScore> data;
    }
}
