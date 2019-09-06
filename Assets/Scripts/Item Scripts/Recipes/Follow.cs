using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Transform target;
    private float AttractorSpeed = 12f;
    private float PullDist = 3f;
    private Coroutine lastCoroutine = null;
    private Rigidbody2D rb;
    private float resetExpireTime = 8f;
    private float ExpireCountDown = 8f;
    private float StartFlashingTime = 3f;
    private float flashDuration = .08f;
    private bool StartedFlashing = false;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (!sprite.enabled)
        {
            sprite.enabled = true;
        }
    }

    void OnDisable()
    {
        StartedFlashing = false;
        ExpireCountDown = resetExpireTime;
    }

    void Update()
    {
        ExpireCountDown -= Time.deltaTime;
        if (ExpireCountDown <= StartFlashingTime && StartedFlashing == false)
        {
            StartedFlashing = true;
            StartCoroutine(ExpiringFlashCo());
        }
        if (ExpireCountDown <= 0)
        {
            ExpireCountDown = resetExpireTime;
            gameObject.SetActive(false);
        }
    }


    void FixedUpdate()
    {
        float PlayerDist = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (lastCoroutine == null && PlayerDist < PullDist)
        {
            if (GetComponent<ItemPickup>().item.itemType == ItemType.Physical)
            {
                if (Inventory.Instance.getPhysicalCount() < 100)
                {
                    MoveTowardPlayer();
                }
            }
            else if (GetComponent<ItemPickup>().item.itemType == ItemType.GunPowder)
            {
                if (Inventory.Instance.getGunpowderCount() < 100)
                {
                    MoveTowardPlayer();
                }
            }
            else if (GetComponent<ItemPickup>().item.itemType == ItemType.Explosive)
            {
                if (Inventory.Instance.getExplosiveCount() < 100)
                {
                    MoveTowardPlayer();
                }
            }
        }
    }

    private void MoveTowardPlayer()
    {
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        rb.velocity = direction * AttractorSpeed;
    }

    IEnumerator ExpiringFlashCo()
    {
        while (StartedFlashing)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(flashDuration);
            sprite.enabled = true;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}

