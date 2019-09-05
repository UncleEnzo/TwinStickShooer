using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh random shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Random Shot")]
public class UbhRandomShot : UbhBaseShot
{
    [Header("===== RandomShot Settings =====")]
    // "Sets weather the bullets should all fire at once as a cluster or as independant shots"
    // "Shoot at once is useful for shotguns"
    public bool shootAtOnce;
    // "Set a angle size of random range. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_RandomRangeSize")]
    public float m_randomRangeSize = 360f;
    // "Set how much faster the bullet speed range can vary."
    [FormerlySerializedAs("_RandomSpeedMax")]
    [Header("This m_randomSpeedMax += bulletSpeed. Sets upper limit of how much faster a bullet can be.")]
    public float m_randomSpeedMax = 1f;
    [Header("Lose delay values if Shoot At Once Enabled.")]
    [Header("Compensate by increasing random size range and speed.")]
    [FormerlySerializedAs("_RandomDelayMin")]
    public float m_randomDelayMin = 0.01f;
    // "Set a maximum delay time between bullet and next bullet. (sec)"
    [FormerlySerializedAs("_RandomDelayMax")]
    public float m_randomDelayMax = 0.1f;
    // "Evenly distribute of all bullet angle."
    [FormerlySerializedAs("_EvenlyDistribute")]
    public bool m_evenlyDistribute = true;

    private float m_delayTimer;

    private List<int> m_numList;

    public override void Shot()
    {
        if (m_bulletNum <= 0 || m_randomSpeedMax <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or RandomSpeedMax is not set.");
            return;
        }

        if (m_shooting)
        {
            return;
        }

        m_shooting = true;
        m_delayTimer = 0f;

        if (m_numList != null)
        {
            m_numList.Clear();
            m_numList = null;
        }

        m_numList = new List<int>(m_bulletNum);
        for (int i = 0; i < m_bulletNum; i++)
        {
            m_numList.Add(i);
        }
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

        if (shootAtOnce)
        {
            int index = Random.Range(0, m_numList.Count);
            distributeAnglesFireShot(index);
            m_numList.RemoveAt(index);
        }
        else
        {
            foreach (int shot in m_numList)
            {
                distributeAnglesFireShot(Random.Range(0, m_numList.Count));
            }
            m_numList.Clear();
        }
        if (m_numList.Count <= 0)
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

    private void distributeAnglesFireShot(int index)
    {
        UbhBullet bullet = GetBullet(transform.position);
        if (bullet == null)
        {
            return;
        }

        float minAngle = m_angle - (m_randomRangeSize / 2f);
        float maxAngle = m_angle + (m_randomRangeSize / 2f);
        float angle = 0f;

        if (m_evenlyDistribute)
        {
            float oneDirectionNum = Mathf.Floor((float)m_bulletNum / 4f);
            float quarterIndex = Mathf.Floor((float)m_numList[index] / oneDirectionNum);
            float quarterAngle = Mathf.Abs(maxAngle - minAngle) / 4f;
            angle = Random.Range(minAngle + (quarterAngle * quarterIndex), minAngle + (quarterAngle * (quarterIndex + 1f)));
        }
        else
        {
            angle = Random.Range(minAngle, maxAngle);
        }

        ShotBullet(m_bulletTag, m_damage, m_knockBack, m_bulletAccuracy, m_isExplosiveRecipe, m_isBulletBounce, m_bulletBounceMaxNum,
                            m_isExplosive, m_explosionDamage, m_explosiveForce, m_explosiveRadius,
                            m_explosionEffect, bullet, Random.Range(m_bulletSpeed, m_bulletSpeed + m_randomSpeedMax), angle);
        FiredShot();
    }
}