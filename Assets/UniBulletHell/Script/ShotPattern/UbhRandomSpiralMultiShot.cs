﻿using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh random spiral multi shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Spiral Multi Shot")]
public class UbhRandomSpiralMultiShot : UbhBaseShot
{
    [Header("===== RandomSpiralMultiShot Settings =====")]
    // "Set a number of shot spiral way."
    [FormerlySerializedAs("_SpiralWayNum")]
    public int m_spiralWayNum = 4;
    // "Set a shift angle of spiral. (-360 to 360)"
    [Range(-360f, 360f), FormerlySerializedAs("_ShiftAngle")]
    public float m_shiftAngle = 5f;
    // "Set a angle size of random range. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_RandomRangeSize")]
    public float m_randomRangeSize = 30f;
    // "Set how much faster the bullet speed range can vary."
    [FormerlySerializedAs("_RandomSpeedMax")]
    [Header("This m_randomSpeedMax += bulletSpeed. Sets upper limit of how much faster a bullet can be.")]
    public float m_randomSpeedMax = 1f;
    // "Set a minimum delay time between bullet and next bullet. (sec)"
    [FormerlySerializedAs("_RandomDelayMin")]
    public float m_randomDelayMin = 0.01f;
    // "Set a maximum delay time between bullet and next bullet. (sec)"
    [FormerlySerializedAs("_RandomDelayMax")]
    public float m_randomDelayMax = 0.1f;

    private int m_nowIndex;
    private float m_delayTimer;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_randomSpeedMax <= 0 || m_spiralWayNum <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or RandomSpeedMax or SpiralWayNum is not set.");
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

            float centerAngle = m_angle + (spiralWayShiftAngle * i) + (m_shiftAngle * Mathf.Floor(m_nowIndex / m_spiralWayNum));
            float minAngle = centerAngle - (m_randomRangeSize / 2f);
            float maxAngle = centerAngle + (m_randomRangeSize / 2f);
            float angle = Random.Range(minAngle, maxAngle);

            ShotBullet(m_bulletTag, m_damage, m_knockBack, m_bulletAccuracy, m_isExplosiveRecipe, m_isBulletBounce, m_bulletBounceMaxNum,
                            m_isExplosive, m_explosionDamage, m_explosiveForce, m_explosiveRadius,
                            m_explosionEffect, bullet, Random.Range(m_bulletSpeed, m_bulletSpeed + m_randomSpeedMax), angle);

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
            m_delayTimer = Random.Range(m_randomDelayMin, m_randomDelayMax);
            if (m_delayTimer <= 0f)
            {
                Update();
            }
        }
    }
}