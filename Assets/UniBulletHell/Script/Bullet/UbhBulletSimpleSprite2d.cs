using UnityEngine;

/// <summary>
/// Ubh bullet for sprite2d and rigidbody2d prefabs.
/// </summary>
public class UbhBulletSimpleSprite2d : UbhBullet
{
    [SerializeField]
    private Rigidbody2D m_rigidbody2d = null;
    [SerializeField]
    private Collider2D[] m_collider2ds = null;
    [SerializeField]
    private SpriteRenderer[] m_spriteRenderers = null;

    private bool m_isActive;

    /// <summary>
    /// Activate/Inactivate flag
    /// Override this property when you want to change the behavior at Active / Inactive.
    /// </summary>
    public override bool isActive { get { return m_isActive; } }

    /// <summary>
    /// Activate/Inactivate Bullet
    /// </summary>
    public override void SetActive(bool isActive)
    {
        m_isActive = isActive;

        m_rigidbody2d.simulated = isActive;

        if (m_collider2ds != null && m_collider2ds.Length > 0)
        {
            for (int i = 0; i < m_collider2ds.Length; i++)
            {
                m_collider2ds[i].enabled = isActive;
            }
        }

        if (m_spriteRenderers != null && m_spriteRenderers.Length > 0)
        {
            for (int i = 0; i < m_spriteRenderers.Length; i++)
            {
                m_spriteRenderers[i].enabled = isActive;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (this.gameObject.tag == TagsAndLabels.PlayerBulletTag && collisionInfo.gameObject.tag == TagsAndLabels.EnemyTag)
        {
            if (!m_isExplosive)
            {
                Rigidbody2D rb = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
                Vector2 difference = rb.transform.position - transform.position;
                difference = difference * m_knockBack;
                collisionInfo.gameObject.GetComponent<Enemy>().hit(m_damage, m_knockBack, difference);
            }
        }
        if (this.gameObject.tag == TagsAndLabels.EnemyBulletTag && collisionInfo.gameObject.tag == TagsAndLabels.PlayerTag)
        {
            if (!m_isExplosive)
            {
                collisionInfo.gameObject.GetComponent<Player>().hit(m_damage);
            }
        }
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.ChestLabel))
        {
            collisionInfo.gameObject.GetComponent<TreasureChest>().health--;
        }
        if (m_isExplosive)
        {
            explosiveBullet();
        }
        if (m_isBulletBounce)
        {
            //Recalculating bullet trajectory for next hit :)
            disableForBounce = true;
            m_bounces++;
            if (m_bounces >= m_bulletBounceMaxNum)
            {
                if (this != null && this.isActive)
                {
                    m_bounces = 0;
                    disableForBounce = false;
                    isBounceSet = false;
                    UbhObjectPool.instance.ReleaseBullet(this);
                }
            }
        }
        if (!m_isBulletBounce)
        {
            if (this != null && this.isActive)
            {
                m_bounces = 0;
                disableForBounce = false;
                isBounceSet = false;
                UbhObjectPool.instance.ReleaseBullet(this);
            }
        }
    }
    protected void explosiveBullet()
    {
        //create explosion
        GameObject explosion = Instantiate(m_explosionEffect, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_explosiveRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            //Note: only reinstate this if you will be only using player explosive bullets
            // //Destroys enemy bullets caught in the explosion
            // if (!nearbyObject.isTrigger && nearbyObject.GetComponent<EnemyBullet>())
            // {
            //     nearbyObject.gameObject.SetActive(false);
            // }

            //Applies explosion
            if (nearbyObject.tag != TagsAndLabels.PlayerBulletTag && !nearbyObject.isTrigger && nearbyObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                Vector2 difference = rb.transform.position - transform.position;
                difference = difference * m_explosiveForce;
                if (rb.GetComponent<Player>())
                {
                    rb.GetComponent<Player>().hit(0, m_explosiveForce, difference);
                }
                //Applies explosive damage to the enemy
                if (rb.GetComponent<Enemy>())
                {
                    rb.GetComponent<Enemy>().hit(m_explosionDamage, m_explosiveForce, difference);
                }
                else
                {
                    rb.AddForce(difference, ForceMode2D.Impulse);
                }
            }
        }
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
    }
}
