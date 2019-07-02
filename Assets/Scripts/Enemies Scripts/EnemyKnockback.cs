using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Enemy enemy;
    public float knockTime = .25f;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public void OnCollisionEnter2D(Collision2D collidingObject)
    {
        if (collidingObject.gameObject.tag == TagsAndLabels.PlayerBulletTag && gameObject.tag == TagsAndLabels.EnemyTag)
        {
            if (gameObject.activeInHierarchy == true)
            {
                enemy.isKnockedBack = true;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                Vector2 difference = collidingObject.gameObject.GetComponent<PlayerBullet>().getBulletTrajectory();
                difference = difference.normalized * collidingObject.gameObject.GetComponent<PlayerBullet>().getBulletKnockBack();
                rb.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(knockCo(rb));
            }
        }
    }

    private IEnumerator knockCo(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
    }
}
