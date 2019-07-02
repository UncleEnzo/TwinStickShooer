using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        Vector2 difference = body.transform.position - explosionPosition;
        difference = difference.normalized * explosionForce;
        body.AddForce(difference, ForceMode2D.Impulse);

        // var dir = (body.transform.position - explosionPosition);
        // float wearoff = 1 - (dir.magnitude / explosionRadius);
        // body.AddForce(dir.normalized * explosionForce * wearoff);
    }
}
