using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadialFire : Enemy
{
    new private void Start()
    {
        base.Start();
        aiPath.canMove = true;
    }
    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (distFromPlayer < 7)
        {
            coolDownOnMovementTimer -= Time.deltaTime;
            if (coolDownOnMovementTimer <= 0)
            {
                GetComponent<UbhShotCtrl>().StartShotRoutine();
                coolDownOnMovementTimer = movementCoolDownReset;
            }
        }
    }
}
