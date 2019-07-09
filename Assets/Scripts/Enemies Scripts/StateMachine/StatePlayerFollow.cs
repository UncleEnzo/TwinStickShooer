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


    public override void EnterState(Enemy owner)
    {
        //animations
        followPlayer(owner);
    }

    public override void ExitState(Enemy owner)
    {

    }

    public override void UpdateState(Enemy owner)
    {

    }

    private void followPlayer(Enemy owner)
    {
        owner.aiPath.canMove = true;
    }
}
