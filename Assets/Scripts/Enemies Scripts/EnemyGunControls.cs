using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunControls : MonoBehaviour
{
    //general variables
    private Transform playerTransform;
    private bool gunFacingRight = true;
    private Vector3 pointAtPlayer;
    public float enemyArmLength = .5f;

    // Update is called once per frame
    void Update()
    {
        playerTransform = Player.Instance.transform;
        gunControls();
        monitorGunSpriteFlip();
    }

    private void monitorGunSpriteFlip()
    {
        //Flips gunsprite over the Y axis
        if (transform.root.InverseTransformPoint(transform.position).x > 0 && !gunFacingRight)
        {
            flipGunSprite();
        }
        else if (transform.root.InverseTransformPoint(transform.position).x < 0 && gunFacingRight)
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
        pointAtPlayer = playerTransform.position;
        lookAtPoint(pointAtPlayer);
        rotateAroundShoulder(pointAtPlayer, enemyArmLength);
    }

    private void lookAtPoint(Vector3 targetTransform)
    {
        Vector3 dir = targetTransform - transform.root.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void rotateAroundShoulder(Vector3 playerLocation, float armLength)
    {
        Vector3 shoulderToMouseDir = playerLocation - transform.root.position;
        shoulderToMouseDir.z = 0;
        transform.position = transform.root.position + (armLength * shoulderToMouseDir.normalized);
    }
}
