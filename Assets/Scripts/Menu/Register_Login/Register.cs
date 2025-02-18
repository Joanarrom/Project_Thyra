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

public class Register : MonoBehaviour
{
    
    public void register()
    {
        Debug.Log("Register...");
        UnityWebRequest httpRequest = new UnityWebRequest();
        httpRequest.method = UnityWebRequest.kHttpVerbPOST;
        httpRequest.uri = loginDataSO.apiUrl + "/Auth/Register";
        httpRequest.SetRequestHeader("Content-Type", "application/json");
        httpRequest.SetRequestHeader("Accept", "application/json");

        RegisterUserDTO registerUserDto = new RegisterUserDTO();
        registerUserDto.Nom = nomInput.text;
        registerUserDto.Email = emailInput.text;
        registerUserDto.Password = passwordInput.text;

        string jsonData = JsonConvert.SerializeObject(registerUserDto);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpRequest.uploadHandler = new UploadHandlerRaw(dataToSend);
        httpRequest.downloadHandler = new DownloadHandlerBuffer();

        httpRequest.SendWebRequest();

        while (!httpRequest.isDone)
        {
            ;
        }

        if (httpRequest.result == UnityWebRequest.Result.ConnectionError || 
            httpRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + httpRequest.error);
            return;
        }

        Debug.Log(httpRequest.result.ToString());

        string jsonResponse = httpRequest.downloadHandler.text;
        UserDTO registeredUser = JsonConvert.DeserializeObject<UserDTO>(jsonResponse);

        Debug.Log("Creat usuari: " + registeredUser.Nom + " " + registeredUser.Email);
    }
}
