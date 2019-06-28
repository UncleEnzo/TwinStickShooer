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
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            sceneLoader.LoadNextScene();
        }
        //Cheat to load gameover screen
        if (Input.GetKeyDown("g"))
        {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            sceneLoader.loadGameOverScene();
        }
        //Cheat to load Dev Testing Scene
        if (Input.GetKeyDown("t"))
        {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            sceneLoader.loadDevTestScene();
        }

        if (Input.GetKeyDown("p"))
        {
            GetComponentInChildren<EnemySpawner>().activateRandomEnemies(10);
        }

        if (Input.GetKeyDown("u"))
        {
            Inventory.instance.AddItem(money);
        }

        if (Input.GetKeyDown("y"))
        {
            Inventory.instance.RemoveItem(money);
        }

        if (Input.GetKeyDown("l"))
        {
            Inventory.instance.AddItem(key);
        }

        if (Input.GetKeyDown("k"))
        {
            Inventory.instance.RemoveItem(key);
        }

        if (Input.GetKeyDown("1"))
        {
            Inventory.instance.AddItem(physical);
        }

        if (Input.GetKeyDown("2"))
        {
            Inventory.instance.AddItem(gunPowder);
        }

        if (Input.GetKeyDown("3"))
        {
            Inventory.instance.AddItem(explosive);
        }
    }
}
