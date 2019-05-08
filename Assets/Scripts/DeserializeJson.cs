using System.Diagnostics;
using System.Runtime.Serialization.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class DeserializeJson
{
    public int id_game;
    public int id_season;
    public int seed;
    public int points;
    public string historics;
    public string status;
    public static DeserializeJson instance;
    private DeserializeJson()
    {
        
    }

    public static void jsonToObject(string json)
    {
           instance = JsonUtility.FromJson<DeserializeJson>(json);
    }

    public static string objectToJson()
    {
        return JsonUtility.ToJson(instance);
    }
}
