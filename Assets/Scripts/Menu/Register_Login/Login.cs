using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public TMP_InputField userInput;
    public TMP_InputField  passwordInput;
    public TMP_InputField emailInput;
    public NetworkingDataScriptableObject loginDataSO;
    private GameObject usuari = null;
    public void login()
    {
        Debug.Log("Login...");
        StartCoroutine(TryLogin());
    }

    private IEnumerator TryLogin()
    {
        if (usuari == null)
        {
            UnityWebRequest httpClient = new UnityWebRequest();
            httpClient.method = UnityWebRequest.kHttpVerbPOST;
            httpClient.url = loginDataSO.apiUrl + "/Auth/Login";
            httpClient.SetRequestHeader("Content-Type", "application/json");
            httpClient.SetRequestHeader("Accept", "application/json");

            RegisterUserDTO loginDataUsuari = new RegisterUserDTO();
            loginDataUsuari.Nom = userInput.text; // IMPORTANT! Can NOT be null!
            loginDataUsuari.Email = emailInput.text;
            loginDataUsuari.Password = passwordInput.text;

            string jsonData = JsonConvert.SerializeObject(loginDataUsuari);
            byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
            httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);
            httpClient.downloadHandler = new DownloadHandlerBuffer();

            yield return httpClient.SendWebRequest();

            if (httpClient.result == UnityWebRequest.Result.ConnectionError || httpClient.result == UnityWebRequest.Result.ProtocolError)
            {
                throw new Exception("Login: " + httpClient.error);
            }

            string jsonResponse = httpClient.downloadHandler.text;
            AuthTokenDto authTokenDto = JsonConvert.DeserializeObject<AuthTokenDto>(jsonResponse);
            loginDataSO.token = authTokenDto.token;
            Debug.Log(authTokenDto.token);

            httpClient.Dispose();
        }
    }
}
