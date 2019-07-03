﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    // Note: Use this base trick to have start in an inheritance setup
    new void Start()
    {
        base.Start();
        isPlayer = false;
    }
    void Update()
    {
        Vector3 gunTransform = transform.position;
        Transform WeilderTransform = gameObject.transform.root;
        Vector3 playerTransform = Player.Instance.transform.position;
        Aim(playerTransform, WeilderTransform.position);
        SpriteFlip(WeilderTransform, gunTransform);
    }

    public void EnemyFireGun()
    {
        FireGun(isPlayer);
    }
}