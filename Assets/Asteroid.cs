using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public float speed = 0.12f;
    public int imgPerSec = 30;
    public float timeSinceLastUpdate;
    public bool isSynch = false;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastUpdate = 0;
        isSynch = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSynch)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, speed * Time.deltaTime);
        }
        else if(Time.time - timeSinceLastUpdate >= 1/imgPerSec)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, speed * (Time.time - timeSinceLastUpdate));

            timeSinceLastUpdate = Time.time;

        }
    }
}
