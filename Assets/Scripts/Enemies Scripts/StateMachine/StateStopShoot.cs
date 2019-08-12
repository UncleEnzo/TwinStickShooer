using UnityEngine;
using StateMachine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class StateStopShoot : State<Enemy>
{
    #region
    private static StateStopShoot _instance;

    private StateStopShoot()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateStopShoot Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateStopShoot();
            }

            return _instance;
        }
    }
    #endregion
    public override void EnterState(Enemy owner)
    {
        //animations
    }

    public override void ExitState(Enemy owner)
    {
    }

    public override void UpdateState(Enemy owner)
    {
        owner.aiPath.canMove = false;
        owner.StartCoroutine(takeAimThenFire(owner));
    }

    IEnumerator takeAimThenFire(Enemy owner)
    {
        owner.knockedBack = true;
        yield return new WaitForSeconds(owner.waitBeforeFire);
        owner.GetComponentInChildren<EnemyGun>().EnemyFireGun();
        owner.knockedBack = false;
    }
}