using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public Dictionary<string, GameObject> enemyTypes;
    public GameObject[] enemies;
    public float minSpawnDistance;
    public float randomSpawnRange;
    public Player player;

    void Start()
    {
        if (randomSpawnRange <= minSpawnDistance)
        {
            Debug.LogError("Random spawn range is less than or equal to the minimum spawn distance from player");
        }
        FindObjectOfType<Player>();
        enemyTypes = new Dictionary<string, GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (!enemyTypes.ContainsKey(enemy.name))
            {
                enemyTypes.Add(enemy.name, enemy);
            }
        }
    }

    public void instantiateEnemy(int numberOfEnemies, string enemyName)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
           Instantiate(enemyTypes[enemyName], randomEnemyPosition(), player.transform.rotation);
        }
 
    }

    public Vector3 randomEnemyPosition() //todo Make this cleaner, reduce rerolling somehow 
    {
        float xRandomRangeFromPlayer = player.transform.position.x + randomSpawnRange;
        float yRandomRangeFromPlayer = player.transform.position.y + randomSpawnRange;
        float randomXPos = Random.Range(-xRandomRangeFromPlayer, xRandomRangeFromPlayer); //+ minSpawnDistance
        float randomYPos = Random.Range(-yRandomRangeFromPlayer, yRandomRangeFromPlayer);
        while (Mathf.Abs(randomXPos) <= player.transform.position.x + minSpawnDistance)
        {
            randomXPos = Random.Range(-xRandomRangeFromPlayer, xRandomRangeFromPlayer);
            print("WAS NOT THE MINIMUM DISTANCE FROM PLAYER, REROLLING X COORDINATE");
        }
        if (Mathf.Abs(randomYPos) <= player.transform.position.y + minSpawnDistance)
        {
            randomYPos = Random.Range(-yRandomRangeFromPlayer, yRandomRangeFromPlayer);
            print("WAS NOT THE MINIMUM DISTANCE FROM PLAYER, REROLLING Y COORDINATE");
        }
        return new Vector3(randomXPos,randomYPos, 1f);
    }
}
