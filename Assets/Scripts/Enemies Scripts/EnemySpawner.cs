using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Notes: Keep sweep range low and radius low (Consider removing sweep entirely and go for larger radius)
    //Notes: Avoid putting ground beneath walls if possible, so objects down spawn in that space inside wall colliders where there is ground
    //Notes: Use larget ranges for min and max range.  The smaller it is, the longer it takes to spawn
    public GameObject[] enemyCollection;
    public float radiusCast = 3f;
    public Vector2 spawnPoint;
    public float randomRangeMin; // Make sure this is negative
    public float randomRangeMax;
    public EnemyRoom enemyRoom;

    public void spawnKillRoomRandomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            bool spawnSucceeded = spawnEnemy(-1);
            if (spawnSucceeded)
            {
                enemyRoom.addEnemyCount();
            }
        }
    }

    public void spawnKillSelectedEnemies(int numberOfEnemies, int enemyToSpawn)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            bool spawnSucceeded = spawnEnemy(enemyToSpawn);
            if (spawnSucceeded)
            {
                enemyRoom.addEnemyCount();
            }
        }
    }

    public void instantiateRandomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            spawnEnemy(-1);
        }
    }

    public void instantiateSelectedEnemies(int numberOfEnemies, int enemyToSpawn)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            spawnEnemy(enemyToSpawn);
        }
    }

    bool spawnEnemy(int enemyToSpawn)
    {
        bool spawnSucceeded = false;
        if (enemyToSpawn > enemyCollection.Length - 1 || enemyToSpawn < -1)
        {
            print("Int enemyToSpawn is out of bounds of the enemyCollections array.");
            return spawnSucceeded;
        }
        if (randomRangeMin > 0)
        {
            print("Your minimum range is a positive value.");
            return spawnSucceeded;
        }
        int safetyNet = 0;
        bool canSpawnHere = false;
        while (!canSpawnHere)
        {
            float spawnPosX = Random.Range(randomRangeMin, randomRangeMax);
            float spawnPosY = Random.Range(randomRangeMin, randomRangeMax);
            Vector2 spawnPos = new Vector2(transform.position.x + spawnPosX, transform.position.y + spawnPosY);
            //NOTE: Ideally you want to make the insides of walls to not have ground on them
            RaycastHit2D[] rayCastResultRight = Physics2D.CircleCastAll(spawnPos, radiusCast, Vector2.right, 2f);
            RaycastHit2D[] rayCastResultLeft = Physics2D.CircleCastAll(spawnPos, radiusCast, Vector2.left, 2f);
            RaycastHit2D[] rayCastResultUp = Physics2D.CircleCastAll(spawnPos, radiusCast, Vector2.up, 2f);
            RaycastHit2D[] rayCastResultDown = Physics2D.CircleCastAll(spawnPos, radiusCast, Vector2.down, 2f);
            if (rayCastResultRight.Length == 1 && rayCastResultRight[0].collider.gameObject.tag == "Ground")
            {
                if (rayCastResultLeft.Length == 1 && rayCastResultLeft[0].collider.gameObject.tag == "Ground")
                {
                    if (rayCastResultUp.Length == 1 && rayCastResultUp[0].collider.gameObject.tag == "Ground")
                    {
                        if (rayCastResultDown.Length == 1 && rayCastResultDown[0].collider.gameObject.tag == "Ground")
                        {
                            if (enemyToSpawn == -1)
                            {
                                int enemy = Random.Range(0, enemyCollection.Length);

                                GameObject newEnemy = ObjectPooler.SharedInstance.GetPooledObject(enemyCollection[enemy].name + "(Clone)");
                                if (newEnemy != null)
                                {
                                    newEnemy.transform.position = spawnPos;
                                    newEnemy.transform.rotation = Quaternion.identity;
                                    newEnemy.SetActive(true);
                                }
                            }
                            else
                            {
                                GameObject newEnemy = ObjectPooler.SharedInstance.GetPooledObject(enemyCollection[enemyToSpawn].name + "(Clone)");
                                if (newEnemy != null)
                                {
                                    newEnemy.transform.position = spawnPos;
                                    newEnemy.transform.rotation = Quaternion.identity;
                                    newEnemy.SetActive(true);
                                }
                            }
                            spawnSucceeded = true;
                            canSpawnHere = true;
                        }
                    }
                }
            }
            if (canSpawnHere)
            {
                return spawnSucceeded;
            }
            safetyNet++;
            if (safetyNet > 200)
            {
                Debug.Log("Too many attempts");
                return spawnSucceeded;
            }

        }
        return spawnSucceeded;
    }
}
