using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGunPosition : MonoBehaviour
{
    void Update()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        WeaponHolderPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), .5f, Player.Instance.transform.position);
    }
    private void WeaponHolderPosition(Vector3 target, float armLength, Vector3 Wielder)
    {
        Vector3 shoulderToMouseDir = target - Wielder;
        shoulderToMouseDir.z = 0;
        transform.position = Wielder + (armLength * shoulderToMouseDir.normalized);
    }
}
