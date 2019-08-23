using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Ubh base shot.
/// Each shot pattern classes inherit this class.
/// </summary>
public abstract class UbhBaseShot : UbhMonoBehaviour
{
    [Header("My Properties | Adjust in GunProperties")]
    // "Sets the bullet tag.  All bullets are player/enemy agnostic until shot."
    [FormerlySerializedAs("_BulletTag")]
    public bool m_bulletTag;
    // "Determines whether the enemy's active bullets get destroyed on death."
    [FormerlySerializedAs("_DestroyBulletsOnDeath")]
    public bool m_destroyBulletsOnDeath;
    // "Set the damage of bullets."
    [FormerlySerializedAs("_Damage")]
    public float m_damage;
    // "Set the knockBack force of bullets."
    [FormerlySerializedAs("_KnockBack")]
    public float m_knockBack;
    // "Set the accuracy of bullets."
    [FormerlySerializedAs("_BulletAccuracy")]
    public float m_bulletAccuracy;
    // "Sets whether bullets bounce."
    [FormerlySerializedAs("_IsBulletBounce")]
    public bool m_isBulletBounce;
    // "Sets the maximum number of bullet bounces."
    [FormerlySerializedAs("_BulletBounceMaxNum")]
    public int m_bulletBounceMaxNum;
    // "Sets whether bullets are explosive."
    [FormerlySerializedAs("_IsExplosive")]
    public bool m_isExplosive;
    // "Sets bullet explosion damage."
    [FormerlySerializedAs("_ExplosionDamage")]
    public float m_explosionDamage;
    // "Sets bullet explosion force."
    [FormerlySerializedAs("_ExplosiveForce")]
    public float m_explosiveForce;
    // "Sets bullet explosion radius."
    [FormerlySerializedAs("_ExplosiveRadius")]
    public float m_explosiveRadius;
    // "Sets bullet explosion effect."
    [FormerlySerializedAs("_ExplosionEffect")]
    public GameObject m_explosionEffect;

    [Header("===== Common Settings =====")]
    // "Set a bullet prefab for the shot. (ex. sprite or model)"
    [FormerlySerializedAs("_BulletPrefab")]
    public GameObject m_bulletPrefab;
    // "Set a bullet number of shot."
    [FormerlySerializedAs("_BulletNum")]
    public int m_bulletNum = 10;
    // "Set a angle of shot. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_Angle")]
    public float m_angle = 180f;
    // "Set a bullet base speed of shot."
    [FormerlySerializedAs("_BulletSpeed")]
    public float m_bulletSpeed = 2f;
    // "Set an acceleration of bullet speed."
    [FormerlySerializedAs("_AccelerationSpeed")]
    public float m_accelerationSpeed = 0f;
    // "Use max speed flag."
    public bool m_useMaxSpeed = false;
    // "Set a bullet max speed of shot."
    [UbhConditionalHide("m_useMaxSpeed")]
    public float m_maxSpeed = 0f;
    // "Use min speed flag"
    public bool m_useMinSpeed = false;
    // "Set a bullet min speed of shot."
    [UbhConditionalHide("m_useMinSpeed")]
    public float m_minSpeed = 0f;
    // "Set an acceleration of bullet turning."
    [FormerlySerializedAs("_AccelerationTurn")]
    public float m_accelerationTurn = 0f;
    // "This flag is pause and resume bullet at specified time."
    [FormerlySerializedAs("_UsePauseAndResume")]
    public bool m_usePauseAndResume = false;
    // "Set a time to pause bullet."
    [FormerlySerializedAs("_PauseTime"), UbhConditionalHide("m_usePauseAndResume")]
    public float m_pauseTime = 0f;
    // "Set a time to resume bullet."
    [FormerlySerializedAs("_ResumeTime"), UbhConditionalHide("m_usePauseAndResume")]
    public float m_resumeTime = 0f;
    // "This flag is automatically release the bullet GameObject at the specified time."
    [FormerlySerializedAs("_UseAutoRelease")]
    public bool m_useAutoRelease = true;
    // "Set a time to automatically release after the shot at using UseAutoRelease. (sec)"
    [FormerlySerializedAs("_AutoReleaseTime"), UbhConditionalHide("m_useAutoRelease")]
    public float m_autoReleaseTime = 10f;

    public List<UbhBullet> m_activeBullets = new List<UbhBullet>();

    [Space(10)]

    // "Set a callback method fired shot."
    public UnityEvent m_shotFiredCallbackEvents = new UnityEvent();
    // "Set a callback method after shot."
    public UnityEvent m_shotFinishedCallbackEvents = new UnityEvent();

    protected bool m_shooting;

    private UbhShotCtrl m_shotCtrl;

    public UbhShotCtrl shotCtrl
    {
        get
        {
            if (m_shotCtrl == null)
            {
                m_shotCtrl = transform.GetComponentInParent<UbhShotCtrl>();
            }
            return m_shotCtrl;
        }
    }

    /// <summary>
    /// is shooting flag.
    /// </summary>
    public bool shooting { get { return m_shooting; } }

    /// <summary>
    /// is lock on shot flag.
    /// </summary>
    public virtual bool lockOnShot { get { return false; } }

    /// <summary>
    /// Call from override OnDisable method in inheriting classes.
    /// Example : protected override void OnDisable () { base.OnDisable (); }
    /// </summary>
    protected virtual void OnDisable()
    {
        m_shooting = false;
    }

    /// <summary>
    /// Abstract shot method.
    /// </summary>
    public abstract void Shot();

    /// <summary>
    /// UbhShotCtrl setter.
    /// </summary>
    public void SetShotCtrl(UbhShotCtrl shotCtrl)
    {
        m_shotCtrl = shotCtrl;
    }

    /// <summary>
    /// Fired shot.
    /// </summary>
    protected virtual void FiredShot()
    {
        m_shotFiredCallbackEvents.Invoke();
    }

    /// <summary>
    /// Finished shot.
    /// </summary>
    public virtual void FinishedShot()
    {
        m_shooting = false;
        m_shotFinishedCallbackEvents.Invoke();
    }

    /// <summary>
    /// Get UbhBullet object from object pool.
    /// </summary>
    protected UbhBullet GetBullet(Vector3 position, bool forceInstantiate = false)
    {
        if (m_bulletPrefab == null)
        {
            Debug.LogWarning("Cannot generate a bullet because BulletPrefab is not set.");
            return null;
        }

        // get UbhBullet from ObjectPool
        UbhBullet bullet = UbhObjectPool.instance.GetBullet(m_bulletPrefab, position, forceInstantiate);
        if (bullet == null)
        {
            return null;
        }
        m_activeBullets.Add(bullet);
        return bullet;
    }

    /// <summary>
    /// Shot UbhBullet object.
    /// </summary>
    public void ShotBullet(bool bulletTag, float damage, float knockBack, float bulletAccuracy, bool isBulletBounce, int bulletBounceMaxNum,
                            bool isExplosive, float explosionDamage, float explosiveForce, float explosiveRadius,
                            GameObject explosionEffect, UbhBullet bullet, float speed, float angle,
                            bool homing = false, Transform homingTarget = null, float homingAngleSpeed = 0f,
                            bool sinWave = false, float sinWaveSpeed = 0f, float sinWaveRangeSize = 0f, bool sinWaveInverse = false)
    {
        if (bullet == null)
        {
            return;
        }
        bullet.Shot(bulletTag, damage, knockBack, bulletAccuracy, isBulletBounce, bulletBounceMaxNum,
                    isExplosive, explosionDamage, explosiveForce, explosiveRadius, explosionEffect,
                    this, speed, angle, m_accelerationSpeed, m_accelerationTurn,
                    homing, homingTarget, homingAngleSpeed,
                    sinWave, sinWaveSpeed, sinWaveRangeSize, sinWaveInverse,
                    m_usePauseAndResume, m_pauseTime, m_resumeTime,
                    m_useAutoRelease, m_autoReleaseTime,
                    m_shotCtrl.m_axisMove, m_shotCtrl.m_inheritAngle,
                    m_useMaxSpeed, m_maxSpeed, m_useMinSpeed, m_minSpeed);
    }
}