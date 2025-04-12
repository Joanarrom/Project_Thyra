using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoginData", menuName = "ScriptableObjects/NetworkingManagerScriptableObject", order = 1)]

public class NetworkingDataScriptableObject : ScriptableObject
{
    public string apiUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api";
    public string token = "UN055LmAO0OohtenSN8h0y4gB40wsRTYVZq1OkEOOpIukOXrxjKjeUV7Ftbq";
}