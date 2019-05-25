using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkDamageToPlayer = 2;
    public float moveSpeed;
    private Player player;
    private bool walking = true;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (walking)
        {
            approachPlayer();
        }

        shootAtPlayer();
    }

    private void shootAtPlayer()
    {
        GetComponentInChildren<EnemyGunFire>().fireEnemyGun();
    }

    private void approachPlayer()
    {
        Vector3 destination = player.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}
