using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    public int Cost = 0;
    public string DamageDescription = "Damage";
    public string EffectDescription = "Shoots";

    new void Start()
    {
        base.Start();
        isPlayer = true;
    }

    new void Update()
    {
        base.Update();
        if (Player.Instance.playerUsable)
        {
            Aim(mousePosTarget, playerTransform.position);
            SpriteFlip(playerTransform, mousePosTarget);
            FireGun(isPlayer);
        }
    }
}
