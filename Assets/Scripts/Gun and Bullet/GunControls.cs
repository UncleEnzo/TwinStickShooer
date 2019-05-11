using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControls : MonoBehaviour
{
    //determine if gun is attached to player
    bool isPlayerGun = false;

    //general variables
    private Transform shoulder;
    private float lastfired;

    //player only
    private CameraController cam;
    private Vector3 pointAtMouse;
    private float playerArmLength = .5f;

    //enemy only
    private Vector3 pointAtPlayer;
    private float enemyArmLength = .5f;

    //Properties for the current gun
    GunProperties gunProperties;

    //Properties for the bullets and their properties
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        gunProperties = GetComponent<GunProperties>();
        shoulder = transform.parent.transform;
        if (GetComponentInParent<Player>())
        {
            isPlayerGun = true;
            //instantiate the camera controller
            cam = FindObjectOfType<CameraController>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        gunControls();
    }

    public void fireGun()
    {
        if ((Time.time - lastfired) > (1 / gunProperties.bulletsPerSecond))
        {
            lastfired = Time.time;
            foreach (Transform bulletShot in gunProperties.bulletSpawnPoint)
            Instantiate(bullet, bulletShot.position, bulletShot.rotation);
            if (isPlayerGun)
            {
                cam.Shake((transform.parent.transform.position - transform.position).normalized, gunProperties.camShakeMagnitude, gunProperties.camShakeLength);
            }
        }
    }

    private void gunControls()
    {
        if (isPlayerGun)
        {
            pointAtMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lookAtPoint(pointAtMouse);
            rotateAroundShoulder(pointAtMouse, playerArmLength);
        }
        else
        {
            pointAtPlayer = FindObjectOfType<Player>().GetComponentInParent<Transform>().position;
            lookAtPoint(pointAtPlayer);
            rotateAroundShoulder(pointAtPlayer, enemyArmLength);
        }
    }

    private void lookAtPoint(Vector3 point)
    {
        var dir = point - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void rotateAroundShoulder(Vector3 point, float armLength)
    {
        Vector3 shoulderToMouseDir = point - shoulder.position;
        shoulderToMouseDir.z = 0;
        transform.position = shoulder.position + (armLength * shoulderToMouseDir.normalized);
    }
}
