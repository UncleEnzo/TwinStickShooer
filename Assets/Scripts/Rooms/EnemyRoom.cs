using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    private List<Door> doors = new List<Door>();
    public SignalListener enemyUpdate;
    private int enemyCount = 0;
    void Start()
    {
        Door[] doorFullList = FindObjectsOfType<Door>();
        foreach (Door door in doorFullList)
        {
            if (door.thisDoorType == DoorType.enemy)
            {
                //Opens all enemy room doors and adds them to a list
                if (!door.open)
                {
                    door.Open();
                }
                doors.Add(door);
            }
        }
    }

    void Update()
    {
        bool enemiesDefeated = checkEnemyCount();
        if (enemiesDefeated)
        {
            OpenDoors();
        }
        else
        {
            CloseDoors();
        }
    }

    private bool checkEnemyCount()
    {
        if (enemyCount == 0)
        {
            return true;
        }
        return false;
    }

    public void addEnemyCount()
    {
        enemyCount++;
    }

    //Called by enemy when it dies
    public void reduceEnemyCount()
    {
        enemyCount--;
    }
    public void CloseDoors()
    {
        foreach (Door door in doors)
        {
            door.Close();
        }
    }
    public void OpenDoors()
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
    }
}
