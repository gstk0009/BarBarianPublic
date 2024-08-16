using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolList
{
    Monster,
    Bullet,
    Item,
    DamageUi,
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);  
                obj.SetActive(false);
                obj.transform.SetParent(pool.parent);  // 부모 트랜스폼 설정
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.tag.ToString(), objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        
        if (obj.activeInHierarchy && tag != "Item") // 모든 오브젝트 풀 사용중인 상태
            return null;
        
        return obj;
    }

    public void DeactivateAll()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }
}