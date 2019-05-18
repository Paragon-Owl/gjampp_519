using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterController : MonoBehaviour
{
    public GameObject menuChoice;
    public static CharacterController Instance = new CharacterController();
    [Header("Movement")] [SerializeField] public float speed = 5;

    [Header("Acceleration")] [SerializeField] [Range(0f, 1f)]
    public float baseAcceleration = 0.1f;

    [SerializeField] [Range(0f, 1f)] public float decaySpeed = 0.1f;
    [SerializeField] [Range(0f, 1f)] public float acceleration = 0.2f;

    [Header("Rotation")] [SerializeField] [Range(0, 1000)]
    public int dragSpeed = 30;


    [Header("Basic Upgrade")] public float timeForShot = 0.3f;
    public float basicGunDmgMultplier = 1f;
    public float basicSwordDmgMultplier = 1f;
    public float timeForReload = 0.5f;
    public float timeForAttack = 0.3f;

    //Variable for GUN AMJ Upgrades
    [Header("Special GUN Upgrade")] public float gunBonusMultiplier = 1;
    public bool hasMutishot = false;
    public bool hasChargingShot = false;
    public bool hasAutoGuidShot = false;
    public bool hasFireShot = false;
    public bool hasIceShot = false;
    public bool hasThunderShot = false;
    public bool hasBouncingShot = false;
    public bool hasPiercingShot = false;

    //Other variable for Gun actions
    [Header("Other parameter GUN")] public bool isReloading = false;
    public int maxAmmoAmount = 100;
    public int ammoAmount;
    public bool canShot = true;
    public float gunChargingMultiplier = 1;
    public float stepForChargingShot = 0.01f;
    public float maxGunChargingMultiplier = 2;
    public bool isChargingShot = false;
    public float startShotTime;

    //Variable for SWORD AMJ Upgrades
    [Header("Special SWORD Upgrade")] public bool hasDash = false;
    public bool hasChargingSword = false;
    public bool hasKnockbackSword = false;
    public bool hasFireSword = false;
    public bool hasIceSword = false;
    public bool hasThunderSword = false;
    public bool hasBonusSpeed = false;
    public bool hasCriticalDmg = false;
    public float swordBonusMultiplier = 1f;

    //Other variable for Sword Actions
    [Header("Other parameter SWORD")] public float swordChargingMultiplier = 1;
    public bool isChargingSword = false;
    public float stepForChargingSword = 0.01f;
    public float maxSwordChargingMultiplier = 2;

    public float durationSwordAnimAttack = 0.1f;
    public float startSwordAnimAttackTime;

    public bool canAttack = true;
    public float startAttackTime;
    public float dashPower = 1.5f;
    public List<CapsuleCollider2D> SwordCollider2Ds;
    public float swordDmg = 10f;
    public float speedBonusMultiplier = 2f;
    public float durationSpeedBonus = 0.3f;
    public float startSpeedBonus;

    [SerializeField] public GameObject projectilePrefab;
    private Vector2 minBoundary;
    private Vector2 maxBoundary;
    public Vector2 minBoundaryBorder;
    public Vector2 maxBoundaryBorder;
    private Camera gameCamera;

    private Vector3 direction;
    public int shield = 0;
    private Animator _animator;
    private CharacterController()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        direction = Vector2.zero;
        gameCamera = GameObject.FindGameObjectWithTag("GameMainCamera").GetComponent<Camera>();
        Instance.menuChoice = menuChoice;
        minBoundary = gameCamera.ViewportToWorldPoint(Vector3.forward * 10);
        maxBoundary = gameCamera.ViewportToWorldPoint((Vector3.right + Vector3.up) + (Vector3.forward * 10));

        ammoAmount = maxAmmoAmount;
        SwordCollider2Ds[0].enabled = false;
        SwordCollider2Ds[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        debug_actionfortesting();
        Movement();
        GunActions();
        SwordActions();
    }

    void GunActions()
    {
        if (!hasChargingShot)
        {
            if ((Input.GetButton("Fire1") || ButtonA.pressed) && !(Input.GetButton("Fire2") || ButtonB.pressed))
                Fire();
        }
        else
        {
            if ((Input.GetButton("Fire1") || ButtonA.pressed) && !(Input.GetButton("Fire2") || ButtonB.pressed))
            {
                isChargingShot = true;
                gunChargingMultiplier = gunChargingMultiplier <= maxGunChargingMultiplier
                    ? gunChargingMultiplier + stepForChargingShot
                    : gunChargingMultiplier;
            }

            if (isChargingShot && !(Input.GetButton("Fire1") || ButtonA.pressed) && !(Input.GetButton("Fire2") || ButtonB.pressed))
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

        if (Time.time - startShotTime > timeForShot)
        {
            canShot = true;
        }
    }

    void SwordActions()
    {
        if (!hasChargingSword)
        {
            if ((Input.GetButton("Fire2") || ButtonB.pressed) && !(Input.GetButton("Fire1") || ButtonA.pressed))
                Hit();
        }
        else
        {
            if ((Input.GetButton("Fire2") || ButtonB.pressed) && !(Input.GetButton("Fire1") || ButtonA.pressed))
            {
                isChargingSword = true;
                swordChargingMultiplier = swordChargingMultiplier <= maxSwordChargingMultiplier
                    ? swordChargingMultiplier + stepForChargingSword
                    : swordChargingMultiplier;
            }

            if (isChargingShot && !(Input.GetButton("Fire1") || ButtonA.pressed) && !(Input.GetButtonDown("Fire2") || ButtonB.pressed))
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

        /*if (Input.GetKey(KeyCode.LeftArrow))
            direction.x -= ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
            direction.x += ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            direction.y += ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            direction.y -= ((speed * baseAcceleration) + (speed * acceleration)) * Time.deltaTime;*/

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
            _animator.SetTrigger("Fire");
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
            _animator.SetTrigger("Attack");
        }
    }

    public void activeGunBonusDamage()
    {
        gunBonusMultiplier = 2;
    }

    public void activeSwordBonusDamage()
    {
        swordBonusMultiplier = 2;
    }

    public void applyStatUpgrade(StatUpgradeStruct stat)
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

    public void GunUpgrade(AMJ.GunUpgrade upgrade)
    {
        switch(upgrade)
        {
            case AMJ.GunUpgrade.Autoguide:
                hasAutoGuidShot = true;
                break;
            case AMJ.GunUpgrade.Bouncing:
                hasBouncingShot = true;
                break;
            case AMJ.GunUpgrade.Charging:
                isChargingShot = true;
                break;
            case AMJ.GunUpgrade.Damage:
                activeGunBonusDamage();
                break;
            case AMJ.GunUpgrade.Fire:
                hasFireShot = true;
                break;
            case AMJ.GunUpgrade.Ice:
                hasIceShot = true;
                break;
            case AMJ.GunUpgrade.Multishot:
                hasMutishot = true;
                break;
            case AMJ.GunUpgrade.Piercing:
                hasPiercingShot = true;
                break;
            case AMJ.GunUpgrade.Thunder:
                hasThunderShot = true;
                break;
            default:
                break;
        }
    }

    public void SwordUpgrade(AMJ.SwordUpgrade upgrade)
    {
        switch(upgrade)
        {
            case AMJ.SwordUpgrade.Charging:
                hasChargingSword = true;
                break;
            case AMJ.SwordUpgrade.Critical:
                hasCriticalDmg = true;
                break;
            case AMJ.SwordUpgrade.Damage:
                activeSwordBonusDamage();
                break;
            case AMJ.SwordUpgrade.Dash:
                hasDash = true;
                break;
            case AMJ.SwordUpgrade.Fire:
                hasFireSword = true;
                break;
            case AMJ.SwordUpgrade.Ice:
                hasIceSword = true;
                break;
            case AMJ.SwordUpgrade.Knock:
                hasKnockbackSword = true;
                break;
            case AMJ.SwordUpgrade.Speed:
                hasBonusSpeed = true;
                break;
            case AMJ.SwordUpgrade.Thunder:
                hasThunderSword = true;
                break;
            default:
                break;
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
                    swordDmg * totalMultiplier);
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

    private void debug_actionfortesting()
    {
        Instance.gunBonusMultiplier = gunBonusMultiplier;
        Instance.hasMutishot = hasMutishot;
        Instance.hasChargingShot = hasChargingShot;
        Instance.hasAutoGuidShot = hasAutoGuidShot;
        Instance.hasFireShot = hasFireShot;
        Instance.hasIceShot = hasIceShot;
        Instance.hasThunderShot = hasThunderShot;
        Instance.hasBouncingShot = hasBouncingShot;
        Instance.hasPiercingShot = hasPiercingShot;
    }
}