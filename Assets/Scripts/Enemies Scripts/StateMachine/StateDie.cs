using UnityEngine;
using StateMachine;

public class StateDie : State<Enemy>
{
    #region
    private static StateDie _instance;

    private StateDie()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateDie Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateDie();
            }

            return _instance;
        }
    }
    #endregion
    public override void EnterState(Enemy owner)
    {
        //Play some animation, particles, and sounds
        dropCraftComponents(owner);
        dropKey(owner);
        owner.enemyKilled.Raise();
    }
    public override void ExitState(Enemy owner)
    {
    }
    public override void UpdateState(Enemy owner)
    {
        die(owner);
    }

    private void die(Enemy owner)
    {
        owner.gameObject.SetActive(false);
    }
    private void dropCraftComponents(Enemy owner)
    {
        enableComponents(owner, owner.greenCraftComponent.name);
        enableComponents(owner, owner.purpleCraftComponent.name);
        enableComponents(owner, owner.blackCraftComponent.name);
    }
    private void enableComponents(Enemy owner, string craftComponentName)
    {
        for (int i = 0; i < Random.Range(owner.minDropCount, owner.maxDropCount); i++)
        {
            GameObject newComponent = ObjectPooler.SharedInstance.GetPooledObject(craftComponentName + "(Clone)");
            if (newComponent != null)
            {
                newComponent.transform.position = new Vector2(randomDistFromEnemy(owner.transform.position.x, owner), randomDistFromEnemy(owner.transform.position.y, owner));
                newComponent.transform.rotation = owner.transform.rotation;
                newComponent.SetActive(true);
            }
        }
    }
    private void dropKey(Enemy owner)
    {
        int keyDropCheck = Random.Range(0, 10);
        if (keyDropCheck == 1)
        {
            GameObject newKey = ObjectPooler.SharedInstance.GetPooledObject(owner.key.name + "(Clone)");
            if (newKey != null)
            {
                newKey.transform.position = new Vector2(randomDistFromEnemy(owner.transform.position.x, owner), randomDistFromEnemy(owner.transform.position.y, owner));
                newKey.transform.rotation = owner.transform.rotation;
                newKey.SetActive(true);
            }
        }
    }
    private float randomDistFromEnemy(float pos, Enemy owner)
    {
        return Random.Range(pos - owner.minDropDist, pos + owner.maxDropDist);
    }
}