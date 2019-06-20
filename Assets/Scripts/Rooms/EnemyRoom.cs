using System.Linq;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    private List<GameObject> doors = new List<GameObject>();
    public SignalListener killDoorTriggered;
    public SignalListener enemyUpdate;
    public EnemySpawner enemyspawner;
    public int numOfEnemiesToSpawn = 5;
    public int numRemainingEnemies = 0;
    void Start()
    {
        numOfEnemiesToSpawn = Random.Range(5, 10);
        //opens all kill doors in the map
        Door[] allDoorsInMap = FindObjectsOfType<Door>();
        foreach (Door door in allDoorsInMap)
        {
            if (door.thisDoorType == DoorType.enemy && !door.open)
            {
                door.Open();
            }
        }
    }

    public void GetTileMapData(GameObject tileMap)
    {
        //Cleans out the previous list of doors
        foreach (GameObject door in doors)
        {
            doors.Remove(door);
        }
        //creates a new list of doors for the new tilemap
        foreach (Transform child in tileMap.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                doors.Add(child.gameObject);
            }
        }
    }
    public void killDoorActivate()
    {
        CloseDoors();
        foreach (GameObject door in doors)
        {
            door.GetComponentInChildren<Door>().isTriggerCollider.enabled = false;
        }
        FindObjectOfType<PowerUpController>().timerPaused = false;
        FindObjectOfType<PowerUpUIDrawer>().timerPaused = false;
        enemyspawner.spawnKillRoomRandomEnemies(numOfEnemiesToSpawn);
        //numRemainingEnemies = numOfEnemiesToSpawn;
    }

    public void enemyKilledCount()
    {
        numRemainingEnemies--;
        if (numRemainingEnemies <= 0)
        {
            OpenDoors();
            FindObjectOfType<PowerUpController>().timerPaused = true;
            FindObjectOfType<PowerUpUIDrawer>().timerPaused = true;
            numRemainingEnemies = 0;
        }
        print("ENEMY KILL COUNT GOING DOWN: " + numRemainingEnemies);
    }

    private bool checkEnemyCount()
    {
        if (numOfEnemiesToSpawn == 0)
        {
            return true;
        }
        return false;
    }

    public void CloseDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponentInChildren<Door>().Close();
        }
    }
    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.GetComponentInChildren<Door>().Open();
        }
    }
}
