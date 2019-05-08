using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
    public enum Type
    {
        Stat,
        Gun,
        Sword,
        Other
    }

    public Vector3 targetPosition;

    public Sprite[] spritesList;

    public TMPro.TMP_Text title;
    public TMPro.TMP_Text text;

    private Type _type;

    public Type type { get{ return _type; } }

    public AMJ.GunUpgrade gunUpgrade;
    public AMJ.OtherUpgrade otherUpgrade;
    public AMJ.SwordUpgrade swordUpgrade;

    public Stat.Type statUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3();
    }

    public void Set(AMJ.GunUpgrade upgrade)
    {
        _type = Type.Gun;
        gunUpgrade = upgrade;
        switch (upgrade)
        {
            case AMJ.GunUpgrade.Autoguide:
                title.text = "Self-guided bullet";
                text.text = "Bend projectile trajectory to the closest target";
                break;
            case AMJ.GunUpgrade.Bouncing:
                title.text = "Bouncing laser";
                text.text = "Projectile will never be lost again in space, instead they will bounce and go back to th asteroïd field";
                break;
            case AMJ.GunUpgrade.Charging:
                title.text = "Charging gun";
                text.text = "Charge your shot over time to increase all your effects !";
                break;
            case AMJ.GunUpgrade.Damage:
                title.text = "Damage ++";
                text.text = "Double all the damages dealt with the gun, forever";
                break;
            case AMJ.GunUpgrade.Fire:
                title.text = "Fire upgrade";
                text.text = "Every projectile that hit an asteroïd will burn it to piece, dealing damage over time";
                break;
            case AMJ.GunUpgrade.Ice:
                title.text = "Ice upgrade";
                text.text = "Projectile frost asteroïds to slow them down, you'll be able to deal way more damage";
                break;
            case AMJ.GunUpgrade.Multishot:
                title.text = "Multishot";
                text.text = "Fire 3 projectiles instead of one !";
                break;
            case AMJ.GunUpgrade.Piercing:
                title.text = "Piercing laser";
                text.text = "Every laser shot go through enemies";
                break;
            case AMJ.GunUpgrade.Thunder:
                title.text = "Thunder upgrade";
                text.text = "After 3 consecutive hits on an asteroid will deal more damage";
                break;
        }
    }

    public void Set(AMJ.OtherUpgrade upgrade)
    {
        _type = Type.Other;
        otherUpgrade = upgrade;
        switch (upgrade)
        {
            case AMJ.OtherUpgrade.GameTime:
                title.text = "Game Time";
                text.text = "Add 30 seconds to the play time per player";
                break;
            case AMJ.OtherUpgrade.PointEarned:
                title.text = "Point earned";
                text.text = "Every player after you will be able to get more point";
                break;
            case AMJ.OtherUpgrade.Shield:
                title.text = "Shield";
                text.text = "Adds a shield";
                break;
        }
    }

    public void Set(AMJ.SwordUpgrade upgrade)
    {
        _type = Type.Sword;
        swordUpgrade = upgrade;
        switch (upgrade)
        {
            case AMJ.SwordUpgrade.Charging:
                title.text = "Charged hit";
                text.text = "Charge your hit to inflic more damage";
                break;
            case AMJ.SwordUpgrade.Critical:
                title.text = "Critical hit";
                text.text = "Hit will be able to deal critical damage";
                break;
            case AMJ.SwordUpgrade.Dash:
                title.text = "Dash";
                text.text = "Dash when you start to hit";
                break;
            case AMJ.SwordUpgrade.Damage:
                title.text = "Damage ++";
                text.text = "Double all the damages dealt with the sword, forever";
                break;
            case AMJ.SwordUpgrade.Fire:
                title.text = "Fire upgrade";
                text.text = "Hitting an asteroïd will burn it to piece, dealing damage over time";
                break;
            case AMJ.SwordUpgrade.Ice:
                title.text = "Ice upgrade";
                text.text = "Hitting an asteroïd frost it to slow it down, you'll be able to deal way more damage";
                break;
            case AMJ.SwordUpgrade.Knock:
                title.text = "Knockback";
                text.text = "Hitting an asteroid will knock it back !";
                break;
            case AMJ.SwordUpgrade.Speed:
                title.text = "Speed boost";
                text.text = "Grant a small bonus speed after hitting an asteroid";
                break;
            case AMJ.SwordUpgrade.Thunder:
                title.text = "Thunder upgrade";
                text.text = "After 3 consecutive hits on an asteroid will deal more damage";
                break;
        }
    }

    public void Set(Stat.Type upgrade)
    {

        _type = Type.Stat;
        statUpgrade = upgrade;
        switch (upgrade)
        {
            case Stat.Type.Acceleration:
                title.text = "Acceleration";
                text.text = "Move faster to max speed";
                break;
            case Stat.Type.AttackSpeed:
                title.text = "Attack Speed";
                text.text = "Hit faster with the melee attack";
                break;
            case Stat.Type.DamageShot:
                title.text = "Gun damage";
                text.text = "Deal more damage with your projectiles";
                break;
            case Stat.Type.DamageSword:
                title.text = "Melee damage";
                text.text = "Increase the damages dealt with the sword, forever";
                break;
            case Stat.Type.FireRate:
                title.text = "Fire rate";
                text.text = "Increase fire rate";
                break;
            case Stat.Type.MaxSpeed:
                title.text = "Max Speed";
                text.text = "Move faster at max speed";
                break;
            case Stat.Type.ReloadSpeed:
                title.text = "Reload Speed";
                text.text = "Reload your gun faster";
                break;
        }
    }
}
