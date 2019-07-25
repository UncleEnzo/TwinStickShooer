using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    public int Cost = 0;

    new void Start()
    {
        base.Start();
        isPlayer = true;
    }

    void Update()
    {
        Vector3 mousePosTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform playerTransform = Player.Instance.transform;
        Aim(mousePosTarget, playerTransform.position);
        SpriteFlip(playerTransform, mousePosTarget);
        FireGun(isPlayer);
    }
}
