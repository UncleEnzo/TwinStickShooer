using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControls : MonoBehaviour
{
    //determine if gun is attached to player
    bool isPlayerGun = false;

    //general variables
    private Transform shoulder;

    //player only
    private CameraController cam;
    private Vector3 pointAtMouse;
    private float playerArmLength = .5f;
    private bool gunFacingRight = true;

    //enemy only
    private Vector3 pointAtPlayer;
    private float enemyArmLength = .5f;

    void OnEnable()
    {
        if (GetComponentInParent<Player>())
        {
            isPlayerGun = true;
            cam = GetComponentInParent<CameraController>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        shoulder = transform.parent.transform;
        gunControls();
        monitorGunSpriteFlip();
    }

    private void monitorGunSpriteFlip()
    {
        //Flips gunsprite over the Y axis
        Transform player = gameObject.transform.root;
        if (player.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x - gameObject.transform.localPosition.x > 0 && !gunFacingRight)
        {
            flipGunSprite();
        }
        else if (player.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x - gameObject.transform.localPosition.x < 0 && gunFacingRight)
        {
            flipGunSprite();
        }
    }

    private void flipGunSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().flipY = !gameObject.GetComponent<SpriteRenderer>().flipY;
        gunFacingRight = !gunFacingRight;
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

    private void lookAtPoint(Vector3 mouseTransform)
    {
        var dir = mouseTransform - transform.position;
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
