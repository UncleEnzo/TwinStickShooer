using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGunPosition : MonoBehaviour
{
    private Vector3 cachedgunPos;
    private Quaternion cachedRotation;
    private float armLength = .5f;
    void Update()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cachedRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (Player.Instance.playerUsable)
        {
            transform.rotation = cachedRotation;
        }
        Vector3 shoulderToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.Instance.transform.position;
        shoulderToMouseDir.z = 0;
        cachedgunPos = Player.Instance.transform.position + (armLength * shoulderToMouseDir.normalized);
        if (Player.Instance.playerUsable)
        {
            transform.position = cachedgunPos;
        }
    }
}
