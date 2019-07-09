using UnityEngine;
using StateMachine;

public class StatePlayerFollow : State<Enemy>
{
    #region 
    private static StatePlayerFollow _instance;
    private StatePlayerFollow()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }
    public static StatePlayerFollow Instance
    {
        get
        {
            if (_instance == null)
            {
                new StatePlayerFollow();
            }

            return _instance;
        }
    }
    #endregion


    public override void EnterState(Enemy _owner)
    {
        Debug.Log("Entering Follow Player State");
    }

    public override void ExitState(Enemy _owner)
    {
        Debug.Log("Exiting Follow Player State");

    }

    public override void UpdateState(Enemy owner)
    {
        owner.enemyTrajectory = owner.rb.velocity;
        float distFromPlayer = Vector3.Distance(Player.Instance.transform.position, owner.transform.position);
        followPlayer(distFromPlayer, owner);
    }

    private void followPlayer(float distFromPlayer, Enemy owner)
    {
        if (!owner.preparingToFire)
        {
            if (distFromPlayer <= owner.stopAndFireRange)
            {
                owner.aiPath.canMove = false;
            }
            else
            {
                owner.aiPath.canMove = true;
            }
        }
        else
        {
            owner.aiPath.canMove = false;
        }
    }
}
