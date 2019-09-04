using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.WallTag))
        {
            PlayerGun.weaponInWall = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(TagsAndLabels.WallTag))
        {
            PlayerGun.weaponInWall = false;
        }
    }
}
