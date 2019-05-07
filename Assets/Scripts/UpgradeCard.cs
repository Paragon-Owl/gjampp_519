using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
    public Vector3 targetPosition;

    public Sprite[] spritesList;

    public TMPro.TMP_Text title;
    public TMPro.TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3();
    }

    public void Set(AMJ.GunUpgrade upgrade)
    {
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
                text.text = "With the power of electricity, do something strong as always";
                break;
        }
    }

    public void Set(AMJ.OtherUpgrade upgrade)
    {

    }

    public void Set(AMJ.SwordUpgrade upgrade)
    {

    }

    public void Set(Stat.Type upgrade)
    {

    }
}
