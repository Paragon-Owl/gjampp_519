using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public TMPro.TMP_Text scoreTExt;

    public static GameManager instance = new GameManager();
    public const int NB_AMJ = 9;
    public const int NB_AMM = 80;
    public const int NB_LVL_BETWEEN_AMJ = 10;
    public const int NB_LVL = 100;
    public int pointEarned = 0;
    public float pointMultiplier = 1f;
    public int pointPerLevel = 100;

    public List<AMJ.GunUpgrade> gunUpgradeList = new List<AMJ.GunUpgrade>(NB_AMJ);
    public List<AMJ.SwordUpgrade> swordUpgradeList = new List<AMJ.SwordUpgrade>(NB_AMJ);
    public List<AMJ.OtherUpgrade> otherUpgradeList = new List<AMJ.OtherUpgrade>(NB_AMJ);

    public int indexGunUpgradeList = 0;
    public int indexSwordUpgradeList = 0;
    public int indexOtherUpgradeList = 0;

    public List<StatUpgradeStruct> stat1UpgradeList = new List<StatUpgradeStruct>(NB_AMM);
    public List<StatUpgradeStruct> stat2UpgradeList = new List<StatUpgradeStruct>(NB_AMM);
    public List<StatUpgradeStruct> stat3UpgradeList = new List<StatUpgradeStruct>(NB_AMM);

    public int indexStatUpgradeList = 0;
    public string gameScene;
    public float gameTime = 150f;
    public float currentGameTime = 0f;
    public float startGameTime;
    private bool endOfGame = false;
    private string urlRequestPost = "http://makorj.fr/gjpp0519/saveGame.php?PLAYER_NAME=";
    private string urlRequestGet = "http://makorj.fr/gjpp0519/requestGame.php";

    void Awake()
    {
        if(instance == null )
            instance = this;
    }

    private void Start()
    {
        //instance.PlayGame();
        StartCoroutine(LoadMenuScene());
    }

    void Update()
    {
        currentGameTime = Time.time - startGameTime;
        if (currentGameTime > gameTime)
        {
            endOfGame = true;
        }

        scoreTExt.text = (pointEarned/pointPerLevel).ToString();

    }

    IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Scenes/Menu", LoadSceneMode.Additive);
    }

    #region Initialisation

    IEnumerator LoadYourAsyncScene()
    {
        SceneManager.UnloadSceneAsync("Scenes/Menu");
        SceneManager.LoadSceneAsync("Scenes/Loading", LoadSceneMode.Additive);

        UnityWebRequest www = UnityWebRequest.Get(urlRequestGet);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);

            SceneManager.LoadSceneAsync("Scenes/HTTPError", LoadSceneMode.Additive);

            yield break;
        }
        else
        {
            DeserializeJson.jsonToObject(www.downloadHandler.text);
        }

        Debug.Log(DeserializeJson.instance.id_game);
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/Game", LoadSceneMode.Additive);

        PRNG.init(DeserializeJson.instance.seed);
        initMapWithSeed();
        debug_writeUpgradeList();
        loadHistoric(DeserializeJson.instance.historics);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            SceneManager.UnloadSceneAsync("Scenes/Loading");
            yield return null;
        }
        Debug.Log("Loaded");
    }

    private void initMapWithSeed()
    {
        int compteur = 0;
        List<AMJ.GunUpgrade> amjGunAvailable = initMappingIndexGunUpgrade();
        List<AMJ.SwordUpgrade> amjSwordAvailable = initMappingIndexSwordUpgrade();
        List<AMJ.OtherUpgrade> amjOtherAvailable = initMappingIndexOtherUpgrade();
        while (compteur < NB_LVL)
        {
            long rand = PRNG.getNextValue();
            if (compteur != 0 && compteur % NB_LVL_BETWEEN_AMJ == 0)
            {
                int choice = (int) (rand % amjGunAvailable.Count);
                gunUpgradeList.Add(amjGunAvailable[choice]);
                amjGunAvailable.Remove(amjGunAvailable[choice]);
                rand = PRNG.getNextValue();
                choice = (int) (rand % amjSwordAvailable.Count);
                swordUpgradeList.Add(amjSwordAvailable[choice]);
                amjSwordAvailable.Remove(amjSwordAvailable[choice]);
                rand = PRNG.getNextValue();
                choice = (int) (rand % amjOtherAvailable.Count);
                otherUpgradeList.Add(amjOtherAvailable[choice]);
                amjOtherAvailable.Remove(amjOtherAvailable[choice]);
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
    }


    private void loadHistoric(string instanceHistorics)
    {
        int index = 0;
        foreach (char choice in instanceHistorics)
        {
            if (index != 0 && index % NB_LVL_BETWEEN_AMJ == 0)
            {
                switch (choice)
                {
                    case '1':
                        AMJ.applyGunUpgrade(gunUpgradeList[indexGunUpgradeList]);
                        indexGunUpgradeList++;
                        break;
                    case '2':
                        AMJ.applySwordUpgrade(swordUpgradeList[indexSwordUpgradeList]);
                        indexSwordUpgradeList++;
                        break;
                    case '3':
                        AMJ.applyOtherUpgrade(otherUpgradeList[indexOtherUpgradeList]);
                        indexOtherUpgradeList++;
                        break;
                }
            }
            else
            {
                switch (choice)
                {
                    case '1':
                        CharacterController.Instance.applyStatUpgrade(stat1UpgradeList[indexStatUpgradeList]);
                        break;
                    case '2':
                        CharacterController.Instance.applyStatUpgrade(stat2UpgradeList[indexStatUpgradeList]);
                        break;
                    case '3':
                        CharacterController.Instance.applyStatUpgrade(stat3UpgradeList[indexStatUpgradeList]);
                        break;
                }

                indexStatUpgradeList++;
            }

            index++;
        }
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
    private List<AMJ.OtherUpgrade> initMappingIndexOtherUpgrade()
    {
        List<AMJ.OtherUpgrade> dictionnary = new List<AMJ.OtherUpgrade>(NB_AMJ)
        {
            AMJ.OtherUpgrade.GameTime,
            AMJ.OtherUpgrade.GameTime,
            AMJ.OtherUpgrade.GameTime,
            AMJ.OtherUpgrade.PointEarned,
            AMJ.OtherUpgrade.PointEarned,
            AMJ.OtherUpgrade.PointEarned,
            AMJ.OtherUpgrade.Shield,
            AMJ.OtherUpgrade.Shield,
            AMJ.OtherUpgrade.Shield
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

    #region GameManagement

    public void PlayGame()
    {
        StartCoroutine(LoadYourAsyncScene());
        SpawnerController.StartSpawn();
        pointEarned = 10;
        startGameTime = Time.time;
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

    public void debug_writeHistoricAct(string s)
    {
        StreamWriter writer = new StreamWriter("debug.out", true);
        writer.WriteLine(s);
        writer.Close();
    }

    public void debug_writeUpgradeList()
    {
        StreamWriter writer = new StreamWriter("debug.out", true);
        string gun = "GunUpgrade : \n";
        string sword = "SwordUpgrade : \n";
        string basic1 = "StatUpgrade 1 : \n";
        string basic2 = "StatUpgrade 2 : \n";
        string basic3 = "StatUpgrade 3 : \n";
        for (int i = 0; i < 9; i++)
        {
            gun += gunUpgradeList[i] + "\n";
            sword += swordUpgradeList[i] + "\n";
        }

        for (int i = 0; i < NB_AMM; i++)
        {
            basic1 += stat1UpgradeList[i].type + "\n";
            basic2 += stat2UpgradeList[i].type + "\n";
            basic3 += stat3UpgradeList[i].type + "\n";
        }

        writer.Write(gun);
        writer.Write(sword);
        writer.Write(basic1);
        writer.Write(basic2);
        writer.Write(basic3);
        writer.Close();
    }

    #endregion
}