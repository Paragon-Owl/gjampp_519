using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] public static float speed = 5;

    [Header("Acceleration")] [SerializeField] [Range(0f, 1f)]
    public static float acceleration = 0.2f;

    [SerializeField] [Range(0f, 1f)] public static float baseAcceleration = 0.1f;
    [SerializeField] [Range(0f, 1f)] public float decaySpeed = 0.1f;

    [Header("Rotation")] [SerializeField] [Range(0, 360)]
    public int dragSpeed = 30;


    [SerializeField] public GameObject projectilePrefab;


    private Vector2 minBoundary;
    private Vector2 maxBoundary;
    public Vector2 minBoundaryBorder;
    public Vector2 maxBoundaryBorder;
    private Camera gameCamera;

    private Vector3 direction;

    public static float gunBonusMultiplier = 1;
    public static bool hasMutishot = false;


    public static float basicGunDmgMultplier = 1f;
    public static float basicSwordDmgMultplier = 1f;
    
    public static float timeForReload = 0.5f;
    [Header("Reload")]
    [SerializeField] public bool isReloading = false;
    [SerializeField] public int maxAmmoAmount = 100;
    [SerializeField] public int ammoAmount;
    
    
    public static float timeForShot = 0.3f;
    [Header("FireRate")] [SerializeField]
    public bool canShot = true;
    public float startShotTime;

    
    public static bool hasChargingShot = false;
    [Header("CharginShot")] 
    [SerializeField] private float chargingMultiplier = 1;
    [SerializeField] private float maxChargingMultiplier = 2;
    [SerializeField] private float stepForChargingShot = 0.01f;
    [SerializeField] private bool isChargingShot = false;

    public static float timeForAttack = 0.3f;
    [Header("FireRate")] [SerializeField]
    public bool canAttack = true;
    public float startAttackTime;
    
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
        maxBoundary = gameCamera.ViewportToWorldPoint((Vector3.right + Vector3.up) + (Vector3.forward * 10));
       
        ammoAmount = maxAmmoAmount;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (!hasChargingShot)
        {
            if (Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
                Fire();
        }
        else
        {
            if (Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                isChargingShot = true;
                chargingMultiplier = chargingMultiplier <= maxChargingMultiplier
                    ? chargingMultiplier + stepForChargingShot
                    : chargingMultiplier;
            }

            if (isChargingShot && !Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                Fire();
                chargingMultiplier = 1;
                isChargingShot = false;
            }
        }

        if (ammoAmount <= 0)
        {
            StartCoroutine(reload());
        }
        
        if (!hasChargingSword)
        {
            if (Input.GetButton("Fire2") && !Input.GetButtonDown("Fire1"))
                Hit();
        }
        else
        {
            if (Input.GetButton("Fire2") && !Input.GetButtonDown("Fire1"))
            {
                isChargingSword = true;
                chargingMultiplier = chargingMultiplier <= maxChargingMultiplier
                    ? chargingMultiplier + stepForChargingSword
                    : chargingMultiplier;
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
        direction.x += (val * ((speed * baseAcceleration) + (speed * acceleration))) * Time.deltaTime;
        val = Input.GetAxis("Vertical");
        direction.y += (val * ((speed * baseAcceleration) + (speed * acceleration))) * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
            direction.x -= ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            direction.x += ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            direction.y += ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            direction.y -= ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;

        direction.x = Mathf.Clamp(direction.x, -speed, speed);
        direction.y = Mathf.Clamp(direction.y, -speed, speed);


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, dragSpeed * Time.deltaTime);


        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x + direction.x, minBoundary.x + minBoundaryBorder.x,
            maxBoundary.x + maxBoundaryBorder.x);
        newPos.y = Mathf.Clamp(newPos.y + direction.y, minBoundary.y + minBoundaryBorder.y,
            maxBoundary.y + maxBoundaryBorder.y);
        transform.position = newPos;
    }

    void Fire()
    {
        if (!isReloading && canShot)
        {
            if (hasMutishot)
            {
                for (float position = -1; position <= 1; position += 1)
                {
                    double alpha = ((5 + position) * Math.PI) / 10;
                    Vector3 dir = new Vector3((float) Math.Cos(alpha), (float) Math.Sin(alpha), 0);
                    GameObject newShot = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    newShot.GetComponent<Projectile>().direction = dir;
                    newShot.GetComponent<Projectile>().dmg *= (chargingMultiplier * gunBonusMultiplier * basicGunDmgMultplier);
                }
            }
            else
            {
                GameObject newShot = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newShot.GetComponent<Projectile>().dmg *= (chargingMultiplier * gunBonusMultiplier * basicGunDmgMultplier);
            }

            canShot = false;
            startShotTime = Time.time;
            ammoAmount--;
        } else if (!canShot)
        {
            if (Time.time - startShotTime > timeForShot)
            {
                canShot = true;
            }
        }
    }

    void Hit()
    {
        if (canAttack)
        {
            if (hasDash)
            {
                direction += transform.up * dashPower;
            }

            canAttack = false;
            startAttackTime = Time.time;
        }
        else
        {
            if (Time.time - startAttackTime > timeForAttack)
            {
                canAttack = true;
            }
        }

        
    }

    public static void activeGunBonusDamage()
    {
        gunBonusMultiplier = 2;
    }

    public static void applyStatUpgrade(StatUpgradeStruct stat)
    {
        switch (stat.type)
        {
            case Stat.Type.MaxSpeed:
                speed = speed * stat.value;
                break;
            case Stat.Type.Acceleration:
                baseAcceleration = baseAcceleration * stat.value;
                break;
            case Stat.Type.FireRate:
                timeForShot = timeForShot / stat.value;
                break;
            case Stat.Type.ReloadSpeed:
                timeForReload = timeForReload / stat.value;
                break;
            case Stat.Type.DamageShot:
                basicGunDmgMultplier = basicGunDmgMultplier * stat.value;
                break;
            case Stat.Type.DamageSword:
                basicSwordDmgMultplier = basicSwordDmgMultplier * stat.value;
                break;
            case Stat.Type.AttackSpeed:
                timeForAttack = timeForAttack / stat.value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private IEnumerator reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(timeForReload);
        isReloading = false;
        ammoAmount = maxAmmoAmount;
    }
    
}