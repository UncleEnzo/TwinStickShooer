using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    private bool hasExploded = false;
    public GameObject explosionEffect;

    public float radius = 5f;
    public float force = 3f;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }
    void Explode()
    {
        //create explosion
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D nearbyObject in colliders)
        {
            if (!nearbyObject.isTrigger && nearbyObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
                Vector2 difference = rb.transform.position - transform.position;
                difference = difference * force;
                if (rb.GetComponent<Player>())
                {
                    Player.Instance.movementEnabled = false;
                    StartCoroutine(knockBack(rb));
                }
                //Note: Do not have a point of reference for where the weapon is.CameraController.Instance.Shake((Player.Instance.transform.position - transform.position).normalized, 1f, .03f);
                rb.AddForce(difference, ForceMode2D.Impulse);
                //CURRENT BUG IS THAT THE EXPLOSIVE SCRIPT GETS DESTROYED BEFORE IT CAN FINISH THE ENUMERATOR!!!!!!!!!!!!!!!!!!!!!!!
                //Error is that the player movement overwrites add force, disable movement while he be bouncing.
                //EXPERIMENT WITH JUST INCREASING LINEAR DRAG AND REENABLING WHEN VECTOR SET TO ZERO INSTEAD O USING A TIMER
            }
        }
        //Apply Damage to Targets
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        print("Explosion particles destroyed");
        Destroy(gameObject);
    }
    private IEnumerator knockBack(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(1f);
        Player.Instance.movementEnabled = true;
    }
}
