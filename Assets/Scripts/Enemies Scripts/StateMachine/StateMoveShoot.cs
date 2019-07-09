using UnityEngine;
using StateMachine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class StateMoveShoot : State<Enemy>
{
    #region
    private static StateMoveShoot _instance;

    private StateMoveShoot()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateMoveShoot Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateMoveShoot();
            }

            return _instance;
        }
    }
    #endregion
    public override void EnterState(Enemy owner)
    {
        Debug.Log("Entering Second State");
    }

    public override void ExitState(Enemy owner)
    {
        Debug.Log("Exiting Second State");
    }

    public override void UpdateState(Enemy owner)
    {
        float distFromPlayer = Vector3.Distance(Player.Instance.transform.position, owner.transform.position);
        if (!owner.preparingToFire && distFromPlayer <= owner.walkAndFireRange && distFromPlayer > owner.stopAndFireRange)
        {
            owner.GetComponentInChildren<EnemyGun>().EnemyFireGun();
        }
        if (!owner.preparingToFire && distFromPlayer <= owner.stopAndFireRange)
        {
            owner.StartCoroutine(takeAimThenFire(owner));
        }
    }

    IEnumerator takeAimThenFire(Enemy owner)
    {
        owner.preparingToFire = true;
        yield return new WaitForSeconds(owner.waitBeforeFire);
        owner.GetComponentInChildren<EnemyGun>().EnemyFireGun();
        owner.preparingToFire = false;
    }
}