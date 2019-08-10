﻿using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh hole circle shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Hole Circle Shot")]
public class UbhHoleCircleShot : UbhBaseShot
{
    [Header("===== HoleCircleShot Settings =====")]
    // "Set a size of hole. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_HoleSize")]
    public float m_holeSize = 20f;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
            return;
        }

        if (m_shooting)
        {
            return;
        }

        m_shooting = true;
    }

    private void Update()
    {
        if (m_shooting == false)
        {
            return;
        }

        m_angle = UbhUtil.GetNormalizedAngle(m_angle);
        float startAngle = m_angle - (m_holeSize / 2f);
        float endAngle = m_angle + (m_holeSize / 2f);

        float shiftAngle = 360f / (float)m_bulletNum;

        for (int i = 0; i < m_bulletNum; i++)
        {
            float angle = shiftAngle * i;
            if (startAngle <= angle && angle <= endAngle)
            {
                continue;
            }

            UbhBullet bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                break;
            }

            ShotBullet(m_damage, m_knockBack, m_bulletAccuracy, m_isBulletBounce, m_bulletBounceMaxNum,
                            m_isExplosive, m_explosionDamage, m_explosiveForce, m_explosiveRadius,
                            m_explosionEffect, bullet, m_bulletSpeed, angle);
        }

        FiredShot();

        FinishedShot();
    }
}