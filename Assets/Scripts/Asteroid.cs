using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    
    public float speed = 0.12f;
    public int imgPerSec = 30;
    public float timeSinceLastUpdate;
    public bool isSynch = false;
    public float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10);
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

        if (health <= 0)
        {
            DeserializeJson.instance.points += (int)(GameManager.instance.pointEarned*GameManager.instance.pointMultiplier);
            Destroy(gameObject);
        }
    }

    public void applyDmg(float dmg, float fireDmg, float slowImp, float thunderDmg)
    {
        health -= dmg;
        if (fireDmg > 0)
        {
            StartCoroutine(applyFireDmg(fireDmg));
        }

        if (slowImp > 0)
        {
            StartCoroutine(applySlow(slowImp));
        }

        if (thunderDmg > 0)
        {
            //TODO THUNDER DMG
            Debug.Log("APPLY THUNDER DMG");
        }
    }

    public void applyKnock(float knockForce)
    {
        transform.position = new Vector3(transform.position.x,transform.position.y - knockForce, transform.position.z);
    }
    
    private IEnumerator applySlow(float slowImp)
    {
        speed -= slowImp;
        yield return new WaitForSeconds(5);
        speed += slowImp;
    }

    private IEnumerator applyFireDmg(float fireDmg)
    {
        for (int i = 0; i < 5; i++)
        {
            health -= fireDmg;
            yield return new WaitForSeconds(1);
        }
    }
}
