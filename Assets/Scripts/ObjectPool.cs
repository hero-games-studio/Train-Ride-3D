using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoSingleton<ObjectPool>
{

    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

        [Range(0,3)] public int complexity;
    }

    public List<Pool> pools;
	
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag + "doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        //objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);


        return objectToSpawn;
    }

    public GameObject SpawnFromPool(string tag)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag + "doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = Vector3.up*(-100);
        //objectToSpawn.transform.rotation = Quaternion.identity;

        poolDictionary[tag].Enqueue(objectToSpawn);


        return objectToSpawn;
    }

    public List<string> getTags(int complex){
        List<string> toret = new List<string>();
        foreach (Pool pool in pools)
        {
            if(pool.complexity == complex){
                toret.Add(pool.tag);
            }
        }
        return toret;
    }
}



