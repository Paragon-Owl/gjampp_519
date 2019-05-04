using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public List<GameObject> gunUpgradeList = new List<GameObject>(20);
    static public List<GameObject> swordUpgradeList = new List<GameObject>(20);
    static public List<GameObject> otherUpgradeList = new List<GameObject>(20);

    static public int indexGunUpgradeList = 0;
    static public int indexSwordUpgradeList = 0;
    static public int indexOtherUpgradeList = 0;

    static public List<StatUpgradeStruct> stat1UpgradeList = new List<StatUpgradeStruct>(20);
    static public List<StatUpgradeStruct> stat2UpgradeList = new List<StatUpgradeStruct>(20);
    static public List<StatUpgradeStruct> stat3UpgradeList = new List<StatUpgradeStruct>(20);

    static public int indexStatUpgradeList = 0;

    public string gameScene;

    void Update()
    {
        // Press the space key to start coroutine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    IEnumerator LoadYourAsyncScene()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/Game", LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Loaded");
    }

}
