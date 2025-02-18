using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoginData", menuName = "ScriptableObjects/NetworkingManagerScriptableObject", order = 1)]

public class NetworkingDataScriptableObject : ScriptableObject
{
    public string apiUrl = "https://api-zombies-axgcdcasenevhjcj.spaincentral-01.azurewebsites.net/api";
    public string token;
}