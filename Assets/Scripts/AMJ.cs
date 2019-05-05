using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMJ : MonoBehaviour
{
    public enum GunUpgrade
    {
        Multishot,
        Autoguide,
        Charging,
        Ice,
        Fire,
        Thunder,
        Damage,
        Piercing,
        Bouncing
    }

    public enum SwordUpgrade
    {
        Knock,
        Dash,
        Charging,
        Ice,
        Fire,
        Thunder,
        Damage,
        Critical,
        Speed
    }

    public static void applyGunUpgrade(GunUpgrade gunUpgrade)
    {
        switch (gunUpgrade)
        {
            case GunUpgrade.Multishot:
                CharacterController.hasMutishot = true;
                break;
            case GunUpgrade.Autoguide:
                Projectile.isAutoGuide = true;
                break;
            case GunUpgrade.Charging:
                CharacterController.hasChargingShot = true;
                break;
            case GunUpgrade.Ice:
                Projectile.isIce = true;
                break;
            case GunUpgrade.Fire:
                Projectile.isFire = true;
                break;
            case GunUpgrade.Thunder:
                Projectile.isThunder = true;
                break;
            case GunUpgrade.Damage:
                CharacterController.activeGunBonusDamage();
                break;
            case GunUpgrade.Piercing:
                Projectile.isPiercing = true;
                break;
            case GunUpgrade.Bouncing:
                Projectile.isBouncing = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gunUpgrade), gunUpgrade, null);
        }
    }
}
