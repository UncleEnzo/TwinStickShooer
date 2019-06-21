using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    private List<Vector3> tileWorldLocations = new List<Vector3>();

    public void GetGroundTileMapData(GameObject groundTileMap)
    {
        Tilemap groundTileMap2D = groundTileMap.GetComponent<Tilemap>();
        groundTileMap2D.CompressBounds();
        foreach (var pos in groundTileMap2D.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = groundTileMap2D.CellToWorld(localPlace);
            if (groundTileMap2D.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }
    }

    public void spawnKillRoomRandomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            bool spawnSucceeded = spawnEnemy(-1);
            if (spawnSucceeded)
            {
                EnemyRoom enemyRoom = FindObjectOfType<EnemyRoom>();
                enemyRoom.numRemainingEnemies = enemyRoom.numRemainingEnemies + 1;
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
                //   enemyRoom.addEnemyCount();
            }
        }
    }

    public void activateRandomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            spawnEnemy(-1);
        }
    }

    public void activateSelectedEnemies(int numberOfEnemies, int enemyToSpawn)
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
        int safetyNet = 0;
        bool canSpawnHere = false;
        while (!canSpawnHere)
        {
            int randomSpawnPosIndex = Random.Range(0, tileWorldLocations.Count - 1);
            Vector3 spawnPos = tileWorldLocations[randomSpawnPosIndex];
            RaycastHit2D[] rayCastCheckResult = Physics2D.CircleCastAll(spawnPos, radiusCast, Vector2.right, 0);
            if (rayCastCheckResult.Count() == 0)
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
                    canSpawnHere = true;
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