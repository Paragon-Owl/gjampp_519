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
                CharacterController.Instance.hasMutishot = true;
                break;
            case GunUpgrade.Autoguide:
                Projectile.isAutoGuide = true;
                break;
            case GunUpgrade.Charging:
                CharacterController.Instance.hasChargingShot = true;
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
                CharacterController.Instance.activeGunBonusDamage();
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
                CharacterController.Instance.hasKnockbackSword = true;
                break;
            case SwordUpgrade.Dash:
                CharacterController.Instance.hasDash = true;
                break;
            case SwordUpgrade.Charging:
                CharacterController.Instance.hasChargingSword = true;
                break;
            case SwordUpgrade.Ice:
                CharacterController.Instance.hasIceSword = true;
                break;
            case SwordUpgrade.Fire:
                CharacterController.Instance.hasFireSword = true;
                break;
            case SwordUpgrade.Thunder:
                CharacterController.Instance.hasThunderSword = true;
                break;
            case SwordUpgrade.Damage:
                CharacterController.Instance.activeSwordBonusDamage();
                break;
            case SwordUpgrade.Critical:
                CharacterController.Instance.hasCriticalDmg = true;
                break;
            case SwordUpgrade.Speed:
                CharacterController.Instance.hasBonusSpeed = true;
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
                GameManager.instance.gameTime = GameManager.instance.gameTime + 30;
                break;
            case OtherUpgrade.PointEarned:
                GameManager.instance.pointMultiplier = GameManager.instance.pointMultiplier * 1.5f;
                break;
            case OtherUpgrade.Shield:
                CharacterController.Instance.shield++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(otherUpgrade), otherUpgrade, null);
        }
    }
}