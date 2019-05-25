using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunFire : MonoBehaviour
{
    //general variables
    private float lastfired;
    private bool isReloading = false;
    private int currentAmmo;

    //Properties for the gun and bullet
    private GunProperties gunProperties;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        gunProperties = GetComponent<GunProperties>();
        currentAmmo = gunProperties.maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void onDisable()
    {
        StopCoroutine(reload());
    }

    IEnumerator reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(gunProperties.reloadTime);
        currentAmmo = gunProperties.maxAmmo;
        isReloading = false;
    }

    public void fireEnemyGun()
    {
        if (isReloading)
        {
            return;
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(reload());
            return;
        }
        if ((Time.time - lastfired) > (1 / gunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            foreach (Transform bulletShot in gunProperties.bulletSpawnPoint)
            {
                Instantiate(bullet, bulletShot.position, bulletShot.rotation);
            }
        }
    }
}
