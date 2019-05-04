using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    // Start is called before the first frame update
    private string urlRequestPost = "http://makorj.fr/gjpp0519/saveGame.php";
    private string urlRequestGet = "http://makorj.fr/gjpp0519/requestGame.php";
    private UnityWebRequest request;

    public void Start()
    {
        PRNG.init(989877774);
        for (int i = 0; i < 20; i++)
        {
            Debug.Log(PRNG.getNextValue());
        }
        //StartCoroutine(getGame());
    }

    public IEnumerator sendData()
    {
        string logindataJsonString = "{ \"id_semaine\": 1556973946, \"id_game\": 35, \"player_name\": \"Baben\", \"game_status\": \"Begin\" }";
        var request = new UnityWebRequest(urlRequestPost, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(logindataJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Erro: " + request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            Debug.Log("Status Code: " + request.responseCode);
        }
    }
    
    public IEnumerator getGame()
    {
        UnityWebRequest www = UnityWebRequest.Get(urlRequestGet);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
        }
    }
}