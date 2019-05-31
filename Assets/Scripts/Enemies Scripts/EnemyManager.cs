using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyCollection;
    public float minSpawnDistance;
    public float randomSpawnRange;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        if (randomSpawnRange <= minSpawnDistance)
        {
            Debug.LogError("Random spawn range is less than or equal to the minimum spawn distance from player");
        }
        FindObjectOfType<Player>();
    }


    //note: Need to create a separate functions that spawns a specific enemy
    //need to add some rules to the randomness
    public void instantiateRandomEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomEnemy = Random.Range(0, enemyCollection.Length);
            Instantiate(enemyCollection[randomEnemy], randomEnemyPosition(), player.transform.rotation);
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
        }
        if (Mathf.Abs(randomYPos) <= player.transform.position.y + minSpawnDistance)
        {
            randomYPos = Random.Range(-yRandomRangeFromPlayer, yRandomRangeFromPlayer);
        }
        return new Vector3(randomXPos, randomYPos, 1f);
    }
}
