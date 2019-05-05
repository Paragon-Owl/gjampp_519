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

    public enum OtherUpgrade
    {
        GameTime,
        PointEarned,
        Shield
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

    public static void applySwordUpgrade(SwordUpgrade swordUpgrade)
    {
        switch (swordUpgrade)
        {
            case SwordUpgrade.Knock:
                CharacterController.hasKnockbackSword = true;
                break;
            case SwordUpgrade.Dash:
                CharacterController.hasDash = true;
                break;
            case SwordUpgrade.Charging:
                CharacterController.hasChargingSword = true;
                break;
            case SwordUpgrade.Ice:
                CharacterController.hasIceSword = true;
                break;
            case SwordUpgrade.Fire:
                CharacterController.hasFireSword = true;
                break;
            case SwordUpgrade.Thunder:
                CharacterController.hasThunderSword = true;
                break;
            case SwordUpgrade.Damage:
                CharacterController.activeSwordBonusDamage();
                break;
            case SwordUpgrade.Critical:
                CharacterController.hasCriticalDmg = true;
                break;
            case SwordUpgrade.Speed:
                CharacterController.hasBonusSpeed = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(swordUpgrade), swordUpgrade, null);
        }
    }

    public static void applyOtherUpgrade(OtherUpgrade otherUpgrade)
    {
        switch (otherUpgrade)
        {
            case OtherUpgrade.GameTime:
                GameManager.gameTime = GameManager.gameTime + 30;
                break;
            case OtherUpgrade.PointEarned:
                Asteroid.pointMultiplier = Asteroid.pointMultiplier * 1.5f;
                break;
            case OtherUpgrade.Shield:
                CharacterController.shield++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(otherUpgrade), otherUpgrade, null);
        }
    }
}