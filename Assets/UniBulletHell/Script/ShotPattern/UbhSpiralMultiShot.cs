﻿using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh spiral multi shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Spiral Multi Shot")]
public class UbhSpiralMultiShot : UbhBaseShot
{
    [Header("===== SpiralMultiShot Settings =====")]
    // "Set a number of shot spiral way."
    [FormerlySerializedAs("_SpiralWayNum")]
    public int m_spiralWayNum = 4;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f), FormerlySerializedAs("_ShiftAngle")]
    public float m_shiftAngle = 5f;
    // "Set a delay time between bullet and next bullet. (sec)"
    [FormerlySerializedAs("_BetweenDelay")]
    public float m_betweenDelay = 0.2f;

    private int m_nowIndex;
    private float m_delayTimer;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f || m_spiralWayNum <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or SpiralWayNum is not set.");
            return;
        }

        if (m_shooting)
        {
            return;
        }

        m_shooting = true;
        m_nowIndex = 0;
        m_delayTimer = 0f;
    }
    protected virtual void Update()
    {
        if (m_shooting == false)
        {
            return;
        }

        if (m_delayTimer >= 0f)
        {
            m_delayTimer -= UbhTimer.instance.deltaTime;
            if (m_delayTimer >= 0f)
            {
                return;
            }
        }

        float spiralWayShiftAngle = 360f / m_spiralWayNum;

        for (int i = 0; i < m_spiralWayNum; i++)
        {
            UbhBullet bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                break;
            }

            float angle = m_angle + (spiralWayShiftAngle * i) + (m_shiftAngle * Mathf.Floor(m_nowIndex / m_spiralWayNum));

            ShotBullet(m_bulletTag, m_damage, m_knockBack, m_bulletAccuracy, m_isBulletBounce, m_bulletBounceMaxNum,
                            m_isExplosive, m_explosionDamage, m_explosiveForce, m_explosiveRadius,
                            m_explosionEffect, bullet, m_bulletSpeed, angle);

            m_nowIndex++;
            if (m_nowIndex >= m_bulletNum)
            {
                break;
            }
        }

        FiredShot();

        if (m_nowIndex >= m_bulletNum)
        {
            FinishedShot();
        }
        else
        {
            m_delayTimer = m_betweenDelay;
            if (m_delayTimer <= 0f)
            {
                Update();
            }
        }
    }
}