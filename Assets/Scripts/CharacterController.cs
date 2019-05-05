using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] public float speed = 1;

    [Header("Acceleration")]
    [SerializeField][Range(0f,1f)] public float acceleration = 0.2f;
    [SerializeField][Range(0f,1f)] public float baseAcceleration = 0.1f;
    [SerializeField][Range(0f,1f)] public float decaySpeed = 0.1f;

    [Header("Rotation")]
    [SerializeField][Range(0,360)] public int dragSpeed = 30;


    [SerializeField] public GameObject projectilePrefab;


    private Vector2 minBoundary;
    private Vector2 maxBoundary;
    public Vector2 minBoundaryBorder;
    public Vector2 maxBoundaryBorder;
    private Camera gameCamera;

    private Vector3 direction;

    public float gunBonusMultiplier = 1;
    public bool hasMutishot = false;

    public bool hasChargingShot = false;
    private float chargingMultiplier = 1;
    private float maxChargingMultiplier = 2;
    private float stepForChargingShot = 0.01f;
    private bool isChargingShot = false;

    public bool hasDash = false;
    public bool hasChargingSword = false;
    public bool hasKnockbackSword = false;
    private bool isChargingSword = false;
    private float stepForChargingSword = 0.01f;
    public float dashPower = 1.5f;
    private float swordBonusMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        gameCamera = GameObject.FindGameObjectWithTag("GameMainCamera").GetComponent<Camera>();

        minBoundary = gameCamera.ViewportToWorldPoint(Vector3.forward * 10);
        maxBoundary = gameCamera.ViewportToWorldPoint((Vector3.right + Vector3.up)+(Vector3.forward*10));
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if(!hasChargingShot){
            if(Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
                Fire();
        }
        else
        {
            if (Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                isChargingShot = true;
                chargingMultiplier = chargingMultiplier <= maxChargingMultiplier ? chargingMultiplier + stepForChargingShot : chargingMultiplier;
            }
            if (isChargingShot && !Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                Fire();
                chargingMultiplier = 1;
                isChargingShot = false;
            }
        }

        if(!hasChargingSword){
            if(Input.GetButton("Fire2") && !Input.GetButtonDown("Fire1"))
                Hit();
        }
        else
        {
            if (Input.GetButton("Fire2") && !Input.GetButtonDown("Fire1"))
            {
                isChargingSword = true;
                chargingMultiplier = chargingMultiplier <= maxChargingMultiplier ? chargingMultiplier + stepForChargingSword : chargingMultiplier;
            }
            if (isChargingShot && !Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                Hit();
                chargingMultiplier = 1;
                isChargingSword = false;
            }
        }
    }

    void Movement()
    {
        direction = Vector3.MoveTowards(direction, Vector2.zero, speed * decaySpeed * Time.deltaTime);

        float val = Input.GetAxis("Horizontal");
        direction.x += (val * ( (speed * baseAcceleration) + (speed * acceleration))) * Time.deltaTime;
        val = Input.GetAxis("Vertical");
        direction.y += (val * ( (speed * baseAcceleration) + (speed * acceleration))) * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftArrow))
            direction.x -= ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if(Input.GetKey(KeyCode.RightArrow))
            direction.x += ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if(Input.GetKey(KeyCode.UpArrow))
            direction.y += ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if(Input.GetKey(KeyCode.DownArrow))
            direction.y -= ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;

        direction.x = Mathf.Clamp(direction.x, -speed, speed);
        direction.y = Mathf.Clamp(direction.y, -speed, speed);



        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, dragSpeed * Time.deltaTime);


        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x+direction.x, minBoundary.x + minBoundaryBorder.x, maxBoundary.x + maxBoundaryBorder.x);
        newPos.y = Mathf.Clamp(newPos.y+direction.y, minBoundary.y + minBoundaryBorder.y, maxBoundary.y + maxBoundaryBorder.y);
        transform.position = newPos;
    }

    void Fire()
    {

        if (hasMutishot)
        {
            for (float position = -1; position <=1; position += 1)
            {
                double alpha = ((5 + position) * Math.PI) / 10;
                Vector3 dir = new Vector3((float) Math.Cos(alpha), (float) Math.Sin(alpha), 0);
                GameObject newShot = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newShot.GetComponent<Projectile>().direction= dir;
                newShot.GetComponent<Projectile>().dmg *= (chargingMultiplier * gunBonusMultiplier);
            }
        }
        else
        {
            GameObject newShot = Instantiate(projectilePrefab,transform.position, Quaternion.identity);
            newShot.GetComponent<Projectile>().dmg *= (chargingMultiplier * gunBonusMultiplier);
        }
    }

    void Hit()
    {
        if(hasDash)
        {
            direction += transform.up * dashPower;
        }


    }

    void activeGunBonusDamage()
    {
        gunBonusMultiplier = 2;
    }
}
