using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControls : MonoBehaviour
{
    //general variables
    private Transform playerTransform;
    private CameraController cam;
    private Vector3 pointAtMouse;
    public float playerArmLength = .5f;
    private bool gunFacingRight = true;

    void OnEnable()
    {
        if (GetComponentInParent<WeaponSwitching>())
        {
            cam = FindObjectOfType<CameraController>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        gunControls();
        monitorGunSpriteFlip();
    }

    private void monitorGunSpriteFlip()
    {
        //Flips gunsprite over the Y axis
        if (playerTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x > 0 && !gunFacingRight)
        {
            flipGunSprite();
        }
        else if (playerTransform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x < 0 && gunFacingRight)
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
        pointAtMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookAtPoint(pointAtMouse);
        rotateAroundShoulder(pointAtMouse, playerArmLength);
    }

    private void lookAtPoint(Vector3 mouseTransform)
    {
        Vector3 dir = mouseTransform - playerTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void rotateAroundShoulder(Vector3 mouseTransform, float armLength)
    {
        Vector3 shoulderToMouseDir = mouseTransform - playerTransform.position;
        shoulderToMouseDir.z = 0;
        transform.position = playerTransform.position + (armLength * shoulderToMouseDir.normalized);
    }
}
