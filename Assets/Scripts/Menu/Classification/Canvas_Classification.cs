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


public class Canvas_Classification : MonoBehaviour
{
   public NetworkingDataScriptableObject loginDataSO;
   public GameObject listTile;
   public GameObject leaderboardPanel;
   
   
   public void classification()
   {
      StartCoroutine(GetClassification());
   }

   private IEnumerator GetClassification()
   {
      UnityWebRequest httpRequest = UnityWebRequest.Get(loginDataSO.apiUrl + "/LeaderboardL1/GetClassificationLevel1");
      
      httpRequest.SetRequestHeader("Accept", "application/json");
      httpRequest.SetRequestHeader("Authorization", "bearer " + loginDataSO.token);

      yield return httpRequest.SendWebRequest();

      if (httpRequest.result != UnityWebRequest.Result.Success)
      {
         throw new Exception(httpRequest.error);
      }

      

      var classification = JsonConvert.DeserializeObject<List<GameLevel1Dto>>(httpRequest.downloadHandler.text);
      

      foreach (var gameL1Data in classification)
      {
         GameObject newLine = Instantiate(listTile, leaderboardPanel.transform);
         newLine.GetComponent<TextMeshProUGUI>().text = gameL1Data.NomUsuari + "\t" + gameL1Data.Segons;
         
      }
   }
   
}
