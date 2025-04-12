using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class EnviarPuntuacion : MonoBehaviour
{
    public NetworkingDataScriptableObject networkingData;

    public void Enviar(string nombre, int puntuacion)
    {
        StartCoroutine(EnviarDatos(nombre, puntuacion));
    }

    IEnumerator EnviarDatos(string nombre, int puntuacion)
    {
        string fullUrl = networkingData.apiUrl + "/classification";

        DatosPuntuacion datos = new DatosPuntuacion
        {
            api_token = networkingData.token,
            name = nombre,
            puntuacion = puntuacion
        };

        string json = JsonUtility.ToJson(datos);

        UnityWebRequest request = new UnityWebRequest(fullUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Puntuación enviada correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error al enviar puntuación: " + request.error);
        }
    }

    [System.Serializable]
    public class DatosPuntuacion
    {
        public string api_token;
        public string name;
        public int puntuacion;
    }

     private void Start()
    {
        
        Cursor.lockState = CursorLockMode.None;   
        Cursor.visible = true;                    
    }
}
