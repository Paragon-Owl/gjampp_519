using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Runtime.InteropServices;
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
    public bool fireDmgActive = false;
    [FormerlySerializedAs("fireDmgPerSecond")] public float fireDmg = 1f;

    public float thunderDmg = 15f;
    public int nbOfAttackNeeded = 3;
    public int actualNbAttack;

    public ParticleSystem IceEffect;
    public ParticleSystem FireEffect;
    public ParticleSystem ThunderEffect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10);
        timeSinceLastUpdate = 0;
        isSynch = false;
        baseSpeed = speed;
        IceEffect.Stop();
        FireEffect.Stop();
        ThunderEffect.Stop();
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
            DeserializeJson.instance.points += (int)(GameManager.instance.pointEarnedByAsteroid*GameManager.instance.pointMultiplier);
            Destroy(gameObject);
        }

        applyEffect();
    }

    private void applyEffect()
    {
        if (Time.time - startIceEffect > timeOfIceEffect && CharacterController.Instance.hasIceShot)
        {
            speed = baseSpeed;
            if (IceEffect.isPlaying)
            {
                IceEffect.Stop();
            }
        } 
        if (Time.time - startFireEffect > timeOfHitFireEffect && actualNbFireEffect < nbOfHitFireEffect && fireDmgActive && CharacterController.Instance.hasFireShot)
        {
            health -= fireDmg;
            actualNbFireEffect ++;
            startFireEffect = Time.time;
            FireEffect.Play();
        }

        if (actualNbFireEffect >= nbOfHitFireEffect)
        {
            fireDmgActive = false;
        }

        if (nbOfAttackNeeded == actualNbAttack && CharacterController.Instance.hasThunderShot)
        {
            health -= thunderDmg;
            actualNbAttack = 0;
            ThunderEffect.Play();
        }
    }

    public void applyDmg(float dmg)
    {
        Debug.Log(dmg);
        health -= dmg;
        if (CharacterController.Instance.hasFireShot)
        {
            startFireEffect = Time.time;
            actualNbFireEffect = 0;
            fireDmgActive = true;
        }
        if (CharacterController.Instance.hasIceShot)
        {
            startIceEffect = Time.time;
            speed = speed / slowImpDivisor;
            if(IceEffect.isStopped)
                IceEffect.Play();
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
