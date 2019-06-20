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
    }
}
