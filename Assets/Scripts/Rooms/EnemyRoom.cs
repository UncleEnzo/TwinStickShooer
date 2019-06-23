using System.Linq;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    private List<GameObject> doors;
    public SignalListener killDoorTriggered;
    public SignalListener enemyUpdate;
    public EnemySpawner enemyspawner;
    public int randomNumEnemiesToSpawnRange = 3;
    private int numOfEnemiesToSpawn;
    public int numRemainingEnemies = 0;
    void Start()
    {
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

    public void GetRoomData(GameObject room)
    {
        //Cleans out the previous list of doors
        doors = new List<GameObject>();

        //creates a new list of doors for the new tilemap
        foreach (Transform child in room.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                doors.Add(child.gameObject);
            }
        }
    }
    public void killDoorActivate()
    {
        numOfEnemiesToSpawn = Random.Range(1, randomNumEnemiesToSpawnRange);
        numRemainingEnemies = numOfEnemiesToSpawn;
        CloseDoors();
        foreach (GameObject door in doors)
        {
            door.GetComponentInChildren<Door>().isTriggerCollider.enabled = false;
        }
        FindObjectOfType<PowerUpController>().timerPaused = false;
        FindObjectOfType<PowerUpUIDrawer>().timerPaused = false;
        enemyspawner.spawnKillRoomRandomEnemies(numOfEnemiesToSpawn);
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
