using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    private float startTime;
    private float timeForSpawn = 1f;
    private static bool canSpawn = true;
    private int nbAsteroidBySpawn = 2;
    public GameObject AsteroidPrefab;

    private void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && Time.time - startTime > timeForSpawn)
        {
            for (int i = 0; i < nbAsteroidBySpawn; i++)
            {
                Instantiate(AsteroidPrefab, transform.position + new Vector3(Random.Range(-4f, 4f), 0, 0),
                    Quaternion.identity);
            }
            startTime = Time.time;
        }
    }

    public static void StartSpawn()
    {
        canSpawn = true;
    }

    public static void StopSpawn()
    {
        canSpawn = false;
    }
}