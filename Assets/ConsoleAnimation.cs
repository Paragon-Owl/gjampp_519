using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleAnimation : MonoBehaviour
{
    public Transform pos1, pos2, pos3;
    bool isAnimated;
    bool isPhase2;

    public float speed;

    [Range(0,360)]
    public int rotationSpeed;

    public Renderer render;
    public float cutOffValue;
    public float timeToFlout;

    public float timeStart;

    // Start is called before the first frame update
    void Start()
    {
        isAnimated = false;
        isPhase2 = false;
        cutOffValue = 0;
        timeStart = 0;
        timeToFlout = 1/timeToFlout;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAnimated)
        {
            if(!isPhase2)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos2.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, pos2.rotation, rotationSpeed* Time.deltaTime);

                if(transform.position == pos2.position)
                    isPhase2 = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pos3.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, pos3.rotation, 2*rotationSpeed * Time.deltaTime);


                if(transform.position == pos3.position && transform.rotation == pos3.rotation)
                    isAnimated = true;
            }

            cutOffValue += timeToFlout * Time.deltaTime;
            render.material.SetFloat("_Cutoff", cutOffValue);

            timeStart += Time.deltaTime;
        }
        else
        {
            Debug.Log(timeStart);
        }
    }
}
