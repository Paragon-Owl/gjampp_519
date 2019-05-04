using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int NB_AMJ = 9;
    public const int NB_AMM = 80;
    public const int NB_LVL_BETWEEN_AMJ = 10;
    public const int NB_LVL = 100;


    public static List<AMJ.GunUpgrade> gunUpgradeList = new List<AMJ.GunUpgrade>(NB_AMJ);
    public static List<AMJ.SwordUpgrade> swordUpgradeList = new List<AMJ.SwordUpgrade>(NB_AMJ);
    public static List<GameObject> otherUpgradeList = new List<GameObject>(NB_AMJ);

    public static int indexGunUpgradeList = 0;
    public static int indexSwordUpgradeList = 0;
    public static int indexOtherUpgradeList = 0;

    public static List<StatUpgradeStruct> stat1UpgradeList = new List<StatUpgradeStruct>(NB_AMM);
    public static List<StatUpgradeStruct> stat2UpgradeList = new List<StatUpgradeStruct>(NB_AMM);
    public static List<StatUpgradeStruct> stat3UpgradeList = new List<StatUpgradeStruct>(NB_AMM);

    public static int indexStatUpgradeList = 0;
    public string gameScene;
    private static string urlRequestPost = "http://makorj.fr/gjpp0519/saveGame.php?PLAYER_NAME=";
    private static string urlRequestGet = "http://makorj.fr/gjpp0519/requestGame.php";

    void Update()
    {
        // Press the space key to start coroutine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    #region Initialisation

    IEnumerator LoadYourAsyncScene()
    {
        UnityWebRequest www = UnityWebRequest.Get(urlRequestGet);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            DeserializeJson.jsonToObject(www.downloadHandler.text);
        }

        Debug.Log(DeserializeJson.instance.id_game);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/Game", LoadSceneMode.Additive);

        PRNG.init(DeserializeJson.instance.seed);
        initMapWithSeed();
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Loaded");
    }

    private void initMapWithSeed()
    {
        int compteur = 0;
        List<AMJ.GunUpgrade> amjGunAvailable = initMappingIndexGunUpgrade();
        List<AMJ.SwordUpgrade> amjSwordAvailable = initMappingIndexSwordUpgrade();
        while (compteur < NB_LVL)
        {
            long rand = PRNG.getNextValue();
            if (compteur != 0 && compteur % NB_LVL_BETWEEN_AMJ == 0)
            {
                int choice = (int) (rand % amjGunAvailable.Count);
                Debug.Log("Count : " + choice);
                gunUpgradeList.Add(amjGunAvailable[choice]);
                amjGunAvailable.Remove(amjGunAvailable[choice]);
                rand = PRNG.getNextValue();
                choice = (int) (rand % amjSwordAvailable.Count);
                swordUpgradeList.Add(amjSwordAvailable[choice]);
                amjSwordAvailable.Remove(amjSwordAvailable[choice]);
            }
            else
            {
                StatUpgradeStruct firstStatUpgrade = StatUpgradeStruct.GetFromPool(rand);
                stat1UpgradeList.Add(firstStatUpgrade);
                StatUpgradeStruct secondStatUpgrade = chooseSecondStatUpgrade(firstStatUpgrade);
                stat2UpgradeList.Add(secondStatUpgrade);
                StatUpgradeStruct thirdStatUpgrade = chooseThirdStatUpgrade(firstStatUpgrade, secondStatUpgrade);
                stat3UpgradeList.Add(thirdStatUpgrade);
            }

            compteur++;
        }

        debug_showUpgradeList();
    }

    private List<AMJ.GunUpgrade> initMappingIndexGunUpgrade()
    {
        List<AMJ.GunUpgrade> dictionnary = new List<AMJ.GunUpgrade>(NB_AMJ)
        {
            AMJ.GunUpgrade.Autoguide,
            AMJ.GunUpgrade.Bouncing,
            AMJ.GunUpgrade.Charging,
            AMJ.GunUpgrade.Damage,
            AMJ.GunUpgrade.Fire,
            AMJ.GunUpgrade.Ice,
            AMJ.GunUpgrade.Multishot,
            AMJ.GunUpgrade.Piercing,
            AMJ.GunUpgrade.Thunder
        };
        return dictionnary;
    }

    private List<AMJ.SwordUpgrade> initMappingIndexSwordUpgrade()
    {
        List<AMJ.SwordUpgrade> dictionnary = new List<AMJ.SwordUpgrade>(NB_AMJ)
        {
            AMJ.SwordUpgrade.Charging,
            AMJ.SwordUpgrade.Critical,
            AMJ.SwordUpgrade.Dash,
            AMJ.SwordUpgrade.Fire,
            AMJ.SwordUpgrade.Ice,
            AMJ.SwordUpgrade.Knock,
            AMJ.SwordUpgrade.Speed,
            AMJ.SwordUpgrade.Thunder,
            AMJ.SwordUpgrade.Damage
        };
        return dictionnary;
    }

    private static StatUpgradeStruct chooseSecondStatUpgrade(StatUpgradeStruct firstStatUpgrade)
    {
        long rand = PRNG.getNextValue();
        StatUpgradeStruct secondStatUpgrade = StatUpgradeStruct.GetFromPool(rand);
        while (secondStatUpgrade.type == firstStatUpgrade.type)
        {
            rand = PRNG.getNextValue();
            secondStatUpgrade = StatUpgradeStruct.GetFromPool(rand);
        }

        return secondStatUpgrade;
    }

    private StatUpgradeStruct chooseThirdStatUpgrade(StatUpgradeStruct firstStatUpgrade,
        StatUpgradeStruct secondStatUpgrade)
    {
        long rand = PRNG.getNextValue();
        StatUpgradeStruct thirdStatUpgrade = StatUpgradeStruct.GetFromPool(rand);
        while (thirdStatUpgrade.type == firstStatUpgrade.type || thirdStatUpgrade.type == secondStatUpgrade.type)
        {
            rand = PRNG.getNextValue();
            thirdStatUpgrade = StatUpgradeStruct.GetFromPool(rand);
        }

        return thirdStatUpgrade;
    }

    #endregion

    #region Fin du jeu

    public IEnumerator saveGame(string playerName)
    {
        string jsonString = DeserializeJson.objectToJson();
        UnityWebRequest www = new UnityWebRequest(urlRequestPost + playerName, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Status Code: " + www.responseCode);
        }
    }

    #endregion

    #region Debug

    public void debug_showUpgradeList()
    {
        for (int i = 0; i < 9; i++)
        {
            Debug.Log("GunUpgrade " + i + " : " + gunUpgradeList[i]);
            Debug.Log("SwordUpgrade " + i + " : " + swordUpgradeList[i]);
        }
    }

    #endregion
}