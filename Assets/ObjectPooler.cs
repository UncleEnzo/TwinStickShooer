using System.Runtime.InteropServices.ComTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //This vlass creates a pool of objects that are cached in memory
    //the first time they are instantiaed so that you can draw from the cache
    //instead of instantiating a new one.
    public static bool willGrow = true;
    private static int defaulPoolSizeCount = 20;
    public static Dictionary<string, List<GameObject>> objectPools = new Dictionary<string, List<GameObject>>();

    public static GameObject GetPooledObject(string prefabPath, int defaultStartingPoolSize)
    {
        if (!objectPools.ContainsKey(prefabPath))
        {
            CreateObjectPool(prefabPath, defaultStartingPoolSize);
            return GetPooledObject(prefabPath, defaultStartingPoolSize);
        }
        var pool = objectPools[prefabPath];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        if (willGrow)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            GameObject objectInstance = Instantiate(prefab) as GameObject;
            pool.Add(objectInstance);
            return objectInstance;
        }
        return null;
    }

    public static GameObject GetPooledObject(string prefabPath)
    {
        return GetPooledObject(prefabPath, defaulPoolSizeCount);
    }
    public static void CreateObjectPool(string prefabPath, int defaulPoolSizeCount)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        List<GameObject> objects = new List<GameObject>();
        for (int i = 0; i < defaulPoolSizeCount; i++)
        {
            GameObject instance = Instantiate<GameObject>(prefab);
            objects.Add(instance);
            //instance.hideFlags = HideFlags.HideInHierarchy;
            instance.SetActive(false);
        }

        objectPools.Add(prefabPath, objects);
    }
}
