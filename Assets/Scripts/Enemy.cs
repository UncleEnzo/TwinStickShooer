using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 6f;
    public float moveSpeed = 10f;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        approachPlayer();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        takeDamage(collision);
        die();
    }
    private void die()
    {
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private void takeDamage(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
        
    }

    //todo Moves weirdly fast after a while
    private void approachPlayer()
    {
        Vector3 destination = player.transform.position;
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}
