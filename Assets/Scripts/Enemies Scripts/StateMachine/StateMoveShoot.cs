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
        //animations
    }

    public override void ExitState(Enemy owner)
    {
    }

    public override void UpdateState(Enemy owner)
    {
        owner.GetComponentInChildren<EnemyGun>().EnemyFireGun();
    }
}