using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Runtime.InteropServices;
using NUnit.Compatibility;
using UnityEngine;
using UnityEngine.Serialization;

public class Asteroid : MonoBehaviour
{
    public float speed = 0.12f;
    public int imgPerSec = 30;
    public float timeSinceLastUpdate;
    public bool isSynch = false;
    public float health = 100;

    public float timeOfIceEffect = 5f;
    public float startIceEffect;
    public float slowImpDivisor = 2f;
    public float baseSpeed;
    public float timeOfHitFireEffect = 1f;
    public int nbOfHitFireEffect = 5;
    public int actualNbFireEffect;
    public float startFireEffect;
    [FormerlySerializedAs("fireDmgPerSecond")] public float fireDmg = 1f;

    public float thunderDmg = 15f;
    public int nbOfAttackNeeded = 3;
    public int actualNbAttack;
    

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10);
        timeSinceLastUpdate = 0;
        isSynch = false;
        baseSpeed = speed;
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

        applyEffect();
    }

    private void applyEffect()
    {
        if (Time.time - startIceEffect > timeOfIceEffect && CharacterController.Instance.hasIceShot)
        {
            speed = baseSpeed;
            //STOP ANIM OF ICEs
        } 
        if (Time.time - startFireEffect > timeOfHitFireEffect && actualNbFireEffect < nbOfHitFireEffect && CharacterController.Instance.hasFireShot)
        {
            health -= fireDmg;
            actualNbFireEffect ++;
            startFireEffect = Time.time;
            //START ANIM FIRE
        }

        if (nbOfAttackNeeded == actualNbAttack && CharacterController.Instance.hasThunderShot)
        {
            health -= thunderDmg;
            actualNbAttack = 0;
            //START ANIM THUNDER
        }
    }

    public void applyDmg(float dmg)
    {
        health -= dmg;
        if (CharacterController.Instance.hasFireShot)
        {
            startFireEffect = Time.time;
            actualNbFireEffect = 0;
            //START ANIM FIRE
        }
        if (CharacterController.Instance.hasIceShot)
        {
            startIceEffect = Time.time;
            speed = speed / slowImpDivisor;
            //START ANIM OF ICEs
        }

        if (CharacterController.Instance.hasThunderShot)
        {
            actualNbAttack++;
        }
    }

    public void applyKnock(float knockForce)
    {
        transform.position = new Vector3(transform.position.x,transform.position.y - knockForce, transform.position.z);
    }
    
}
