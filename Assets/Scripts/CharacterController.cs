using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviour
{
    
    //Variable for stats uprgrade 
    public static float speed = 1;
    public static float acceleration = 0.2f;
    public static float timeForShot = 0.3f;
    public static float basicGunDmgMultplier = 1f;
    public static float basicSwordDmgMultplier = 1f;
    public static float timeForReload = 0.5f;
    public static float timeForAttack = 0.3f;

    //Variable for GUN AMJ Upgrades
    public static float gunBonusMultiplier = 1;
    public static bool hasMutishot = false;
    public static bool hasChargingShot = false;
    
    //Other variable for Gun actions
    [Header("Gun")]
    public bool isReloading = false;
    public int maxAmmoAmount = 100;
    public int ammoAmount;
    public bool canShot = true;
    private float gunChargingMultiplier = 1;
    public float stepForChargingShot = 0.01f;
    public float maxGunChargingMultiplier = 2;
    public bool isChargingShot = false;
    public float startShotTime;
    
    //Variable for SWORD AMJ Upgrades
    public static bool hasDash = false;
    public static bool hasChargingSword = false;
    public static bool hasKnockbackSword = false;
    public static bool hasFireSword = false;
    public static bool hasIceSword = false;
    public static bool hasThunderSword = false;
    public static bool hasBonusSpeed = false;
    public static bool hasCriticalDmg = false;
    public static float swordBonusMultiplier = 1f;
    
    //Other variable for Sword Actions
    [Header("SWORD")]
    public float swordChargingMultiplier = 1;
    private bool isChargingSword = false;
    private float stepForChargingSword = 0.01f;
    public float maxSwordChargingMultiplier = 2;
    
    public float durationSwordAnimAttack = 0.1f;
    public float startSwordAnimAttackTime;
    
    public bool canAttack = true;
    public float startAttackTime;
    public float dashPower = 1.5f;
    public List<CapsuleCollider2D> SwordCollider2Ds;
    private float fireDmg = 0.5f;
    private float slowImp = 3f;
    private float thunderDmg = 1f;
    public float swordDmg = 10f;
    public float speedBonusMultiplier = 2f;
    public float durationSpeedBonus = 0.3f;
    public float startSpeedBonus;

    
    [Header("Acceleration")]
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


    

    public static int shield = 0;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        gameCamera = GameObject.FindGameObjectWithTag("GameMainCamera").GetComponent<Camera>();

        minBoundary = gameCamera.ViewportToWorldPoint(Vector3.forward * 10);
        maxBoundary = gameCamera.ViewportToWorldPoint((Vector3.right + Vector3.up) + (Vector3.forward * 10));

        ammoAmount = maxAmmoAmount;
        SwordCollider2Ds[0].enabled = false;
        SwordCollider2Ds[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        GunActions();
        SwordActions();
    }

    void GunActions()
    {
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
                gunChargingMultiplier = gunChargingMultiplier <= maxGunChargingMultiplier
                    ? gunChargingMultiplier + stepForChargingShot
                    : gunChargingMultiplier;
            }

            if (isChargingShot && !Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                Fire();
                gunChargingMultiplier = 1;
                isChargingShot = false;
            }
        }
        if (ammoAmount <= 0)
        {
            StartCoroutine(reload());
        }
    }

    void SwordActions()
    {
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
                swordChargingMultiplier = swordChargingMultiplier <= maxSwordChargingMultiplier
                    ? swordChargingMultiplier + stepForChargingSword
                    : swordChargingMultiplier;
            }

            if (isChargingShot && !Input.GetButton("Fire1") && !Input.GetButtonDown("Fire2"))
            {
                Hit();
                gunChargingMultiplier = 1;
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
                    newShot.GetComponent<Projectile>().dmg *=
                        (gunChargingMultiplier * gunBonusMultiplier * basicGunDmgMultplier);
                }
            }
            else
            {
                GameObject newShot = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                newShot.GetComponent<Projectile>().totalMultiplier =
                    (gunChargingMultiplier * gunBonusMultiplier * basicGunDmgMultplier);
            }

            canShot = false;
            startShotTime = Time.time;
            ammoAmount--;
        }
        else if (!canShot)
        {
            if (Time.time - startShotTime > timeForShot)
            {
                canShot = true;
            }
        }
    }

    void Hit()
    {
        if (hasBonusSpeed && Time.time - startSpeedBonus > durationSpeedBonus)
        {
            speed = speed / speedBonusMultiplier;
        }
        if (Time.time - startAttackTime > durationSwordAnimAttack)
        {
            startAttackTime = Time.time;
            SwordCollider2Ds[0].enabled = false;
            SwordCollider2Ds[1].enabled = false;
        }
        else
        {
            if (canAttack)
            {
                if (hasDash)
                {
                    direction += transform.up * dashPower;
                }

                if (hasBonusSpeed)
                {
                    speed = speed * speedBonusMultiplier;
                    startSpeedBonus = Time.time;
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

            SwordCollider2Ds[0].enabled = true;
            SwordCollider2Ds[1].enabled = true;
        }
    }

    public static void activeGunBonusDamage()
    {
        gunBonusMultiplier = 2;
    }

    public static void activeSwordBonusDamage()
    {
        swordBonusMultiplier = 2;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (SwordCollider2Ds[0].isActiveAndEnabled && SwordCollider2Ds[1].isActiveAndEnabled)
        {
            bool collideWithMeteore = false;
            Collider2D[] colliderWithOther = new Collider2D[10];
            for (int i = 0; i < SwordCollider2Ds.Count; i++)
            {
                SwordCollider2Ds[i].GetContacts(colliderWithOther);
                for (int j = 0; j < 10; j++)
                {
                    if (colliderWithOther[j] != null && colliderWithOther[i].CompareTag("Meteore"))
                    {
                        collideWithMeteore = true;
                        break;
                    }
                }
            }

            if (collideWithMeteore)
            {
                float totalMultiplier = swordChargingMultiplier * basicSwordDmgMultplier * swordBonusMultiplier;
                if (hasCriticalDmg)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        totalMultiplier = totalMultiplier * 2;
                    }
                    
                }
                other.gameObject.GetComponent<Asteroid>().applyDmg(
                    swordDmg * totalMultiplier,
                    hasFireSword ? fireDmg * totalMultiplier : 0, hasIceSword ? slowImp : 0,
                    hasThunderSword ? thunderDmg * totalMultiplier : 0);
                if (hasKnockbackSword)
                {
                    other.gameObject.GetComponent<Asteroid>().applyKnock(2);
                }
            }
        }

        if (other.CompareTag("Meteore"))
        {
            //TODO RESPAWN
        }
    }
}