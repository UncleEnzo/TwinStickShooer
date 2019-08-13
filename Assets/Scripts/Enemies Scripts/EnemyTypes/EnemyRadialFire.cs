using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadialFire : Enemy
{
    private bool readyToFire = false;
    public float fireTimer = 6f;
    public float fireTimerReset = 6f;
    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        knockBackAction();
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        distFromPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
        if (!knockedBack)
        {
            switch (enemyStates)
            {
                case EnemyStates.FollowPlayer:
                    //Case Switching
                    if (distFromPlayer < stopAndFireRange)
                    {
                        enemyStates = EnemyStates.StopShoot;
                    }
                    //functionality of case > Inheritly the functionality is moving and nothing else
                    aiPath.canMove = true;
                    break;
                case EnemyStates.StopShoot:
                    if (distFromPlayer > stopAndFireRange)
                    {
                        aiPath.canMove = true;
                        enemyStates = EnemyStates.FollowPlayer;
                    }
                    //functionality of case
                    aiPath.canMove = false;

                    //Shoot
                    if (distFromPlayer < stopAndFireRange)
                    {
                        //CoolDown for parry
                        if (!readyToFire)
                        {
                            fireTimer -= Time.deltaTime;
                            if (fireTimer <= 0)
                            {
                                readyToFire = true;
                                fireTimer = fireTimerReset;
                            }
                        }
                        else
                        {
                            GetComponentInChildren<UbhShotCtrl>().StartShotRoutine();
                            readyToFire = false;
                        }
                    }
                    break;
                case EnemyStates.Die:
                    break;
            }
        }
        base.FixedUpdate();
    }
}
