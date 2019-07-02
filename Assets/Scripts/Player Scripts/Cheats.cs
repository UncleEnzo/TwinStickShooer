using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public Item key;
    public Item money;
    public Item physical;
    public Item gunPowder;
    public Item explosive;

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
        //Cheat to load Dev Testing Scene
        if (Input.GetKeyDown("t"))
        {
            SceneLoader.loadDevTestScene();
        }

        if (Input.GetKeyDown("p"))
        {
            EnemySpawner.Instance.activateRandomEnemies(10);
        }

        if (Input.GetKeyDown("u"))
        {
            Inventory.Instance.AddItem(money);
        }

        if (Input.GetKeyDown("y"))
        {
            Inventory.Instance.RemoveItem(money);
        }

        if (Input.GetKeyDown("l"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(key);
            }
        }

        if (Input.GetKeyDown("k"))
        {
            Inventory.Instance.RemoveItem(key);
        }

        if (Input.GetKeyDown("1"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(physical);
            }
        }

        if (Input.GetKeyDown("2"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(gunPowder);
            }
        }

        if (Input.GetKeyDown("3"))
        {
            for (int i = 0; i < 10; i++)
            {
                Inventory.Instance.AddItem(explosive);
            }
        }
    }
}
