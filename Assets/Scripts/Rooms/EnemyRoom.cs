using System.Linq;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion
    public static EnemyRoom Instance;
    private List<GameObject> doors;
    public SignalListener killDoorTriggered;
    public SignalListener enemyUpdate;
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
        startTimers();
        cleanExpiredTimers();
        EnemySpawner.Instance.spawnKillRoomRandomEnemies(numOfEnemiesToSpawn);
    }

    private void startTimers()
    {
        PowerUpController.Instance.timerPaused = false;
        PowerUpUIDrawer.Instance.timerPaused = false;
    }

    private void cleanExpiredTimers()
    {
        PowerUpController.Instance.CleanExpiredTimers();
        PowerUpUIDrawer.Instance.CleanExpiredTimers();
    }

    public void enemyKilledCount()
    {
        numRemainingEnemies--;
        if (numRemainingEnemies <= 0)
        {
            OpenDoors();
            PowerUpController.Instance.timerPaused = true;
            PowerUpUIDrawer.Instance.timerPaused = true;
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
