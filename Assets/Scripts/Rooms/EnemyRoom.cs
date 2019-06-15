using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    public EnemyHealth[] enemies;
    public Door[] doors;

    //OnTriggerEnter2D spawn enemies

    //need to create a method that populates the enemies list
    //need to make a method that populates the doors list

    //Called by enemy when it dies
    public void removeEnemyFromList()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.activeInHierarchy) // Need to remove enemy from list
            {
                return;
            }
        }
        OpenDoors();
    }
    public void CloseDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Close();
        }
    }
    public void OpenDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Open();
        }
    }
}
