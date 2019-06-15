using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float distPadding = 3f; //happens to both environment collider and instantiating object
    public GameObject enemy;
    public Collider2D[] colliders;
    public float radius;

    void Update()
    {
        testEnemySpawn();
    }

    private void testEnemySpawn()
    {
        if (Input.GetKeyDown("p"))
        {
            for (int i = 0; i < 20; i++)
            {
                spawnEnemy();
            }
        }
    }

    public void spawnEnemy()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        bool canSpawnHere = false;
        int safetyNet = 0;

        while (!canSpawnHere)
        {
            //Todo Set spawn pause range to dimensions of the OverlapBoxAll size
            float spawnPosX = Random.Range(-8.5f, 9.5f);
            float spawnPosY = Random.Range(-4.5f, 5.5f);
            spawnPos = new Vector3(spawnPosX, spawnPosY, 0);

            //Switch to OverlapBoxAll once prove it works, and for size calculate size of level using tilebounds??
            //Todo Need to deterministically put this Overlap in the center of each room instead of tranform.position.  Again, probably done with tilebounds. 
            colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            bool environmentCanSpawnHere = PreventSpawnOverlapEnvironment(spawnPos, colliders);
            bool objectCanSpawnHere = PreventSpawnOverlapInstantiatingObject(spawnPos, enemy);
            if (objectCanSpawnHere && environmentCanSpawnHere)
            {
                canSpawnHere = true;
            }
            if (canSpawnHere)
            {
                break;
            }
            safetyNet++;
            if (safetyNet > 100)
            {
                Debug.Log("Too many attempts");
                break;
            }
        }

        GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity) as GameObject;
    }

    private bool PreventSpawnOverlapInstantiatingObject(Vector3 spawnPos, GameObject objectToSpawn)
    {
        Collider2D[] colliders = new Collider2D[1];
        colliders[0] = objectToSpawn.GetComponent<Collider2D>();
        return PreventSpawnOverlap(spawnPos, colliders);
    }

    private bool PreventSpawnOverlapEnvironment(Vector3 spawnPos, Collider2D[] colliders)
    {
        return PreventSpawnOverlap(spawnPos, colliders);
    }

    private bool PreventSpawnOverlap(Vector3 spawnPos, Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x + distPadding;
            float height = colliders[i].bounds.extents.y + distPadding;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.y - height;
            float upperExtent = centerPoint.y + height;

            if (spawnPos.x >= leftExtent && spawnPos.x <= rightExtent)
            {
                if (spawnPos.y >= lowerExtent && spawnPos.y <= upperExtent)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
