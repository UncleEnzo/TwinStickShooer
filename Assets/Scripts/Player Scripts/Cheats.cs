using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Cheat to autoswitch to next scene
        if (Input.GetKeyDown("n"))
        {
            SceneLoader.LoadNextScene();
        }
        //Cheat to load gameover screen
        if (Input.GetKeyDown("g"))
        {
            SceneLoader.loadGameOverScene();
        }

        if (Input.GetKeyDown("p"))
        {
            EnemySpawner.Instance.activateRandomEnemies(10);
        }

        if (Input.GetKeyDown("u"))
        {
            Inventory.Instance.AddItem(Inventory.Instance.moneyCoin);
        }

        if (Input.GetKeyDown("y"))
        {
            Inventory.Instance.RemoveItem(Inventory.Instance.moneyCoin);
        }

        if (Input.GetKeyDown("l"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.key);
            }
        }

        if (Input.GetKeyDown("k"))
        {
            Inventory.Instance.RemoveItem(Inventory.Instance.key);
        }

        if (Input.GetKeyDown("1"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.physicalComponent);
            }
        }

        if (Input.GetKeyDown("2"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.gunpowderCompontent);
            }
        }

        if (Input.GetKeyDown("3"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.explosiveComponent);
            }
        }
        if (Input.GetKeyDown("4"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.physicalComponent);
            }
        }

        if (Input.GetKeyDown("5"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.gunpowderCompontent);
            }
        }

        if (Input.GetKeyDown("6"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(Inventory.Instance.explosiveComponent);
            }
        }
    }
}
