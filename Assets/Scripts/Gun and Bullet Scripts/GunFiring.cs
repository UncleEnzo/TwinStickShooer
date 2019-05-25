using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFiring : MonoBehaviour
{
    //general variables
    private CameraController cam;
    private float lastfired;
    private bool isReloading = false;
    private int currentAmmo;
    private Transform player;

    //Properties for the gun and bullet
    private GunProperties gunProperties;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        gunProperties = GetComponent<GunProperties>();
        currentAmmo = gunProperties.maxAmmo;
        cam = FindObjectOfType<CameraController>();
    }

    void OnEnable()
    {
        isReloading = false;
    }

    void onDisable()
    {
        StopCoroutine(reload());
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<Player>().transform;
        firePlayerGun();
    }

    IEnumerator reload()
    {
        isReloading = true;
        print("Reloading");
        yield return new WaitForSeconds(gunProperties.reloadTime);
        currentAmmo = gunProperties.maxAmmo;
        isReloading = false;
    }

    public void firePlayerGun()
    {
        if (isReloading)
        {
            return;
        }
        if (Input.GetKeyDown("r") && currentAmmo < gunProperties.maxAmmo)
        {
            StartCoroutine(reload());
            return;
        }
        if (currentAmmo <= 0)
        {
            StartCoroutine(reload());
            return;
        }
        if (Input.GetMouseButton(0) && (Time.time - lastfired) > (1 / gunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            foreach (Transform bulletShot in gunProperties.bulletSpawnPoint)
            {
                Instantiate(bullet, bulletShot.position, bulletShot.rotation);
            }
            currentAmmo--;
            cam.Shake((player.position - transform.position).normalized, gunProperties.camShakeMagnitude, gunProperties.camShakeLength);
        }
    }
}
