using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveThrower : MonoBehaviour
{
    public float throwForce = 5f;
    public GameObject explosiveToThrow;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowExplosive();
        }
    }

    void ThrowExplosive()
    {
        //Logic to throw the explosive
        // foreach (Transform bulletShot in gunProperties.bulletSpawnPoint)
        // {
        //     GameObject newBullet = ObjectPooler.SharedInstance.GetPooledObject(bullet.name + "(Clone)");
        //     if (newBullet != null)
        //     {
        //         newBullet.transform.position = bulletShot.position;
        //         newBullet.transform.rotation = bulletShot.rotation;
        //         newBullet.SetActive(true);
        //     }
        //     newBullet.GetComponent<PlayerBullet>().setPlayerBulletProperties(gunProperties.bulletSpeed, gunProperties.bulletDamage, gunProperties.timeBulletSelfDestruct, gunProperties.knockBack, gunProperties.bulletAccuracy, gunProperties.bulletAngle, gunProperties.bulletBounce);
        // }



        //Logic to throw the explosive
        GameObject explosive = Instantiate(explosiveToThrow, transform.position, transform.rotation);
        Rigidbody2D rb = explosiveToThrow.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.forward * throwForce);
    }
}
