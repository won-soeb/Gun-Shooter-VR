using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    [System.Serializable]
    public struct BulletPool
    {
        public string bulletType;
        public GameObject bulletPrefab;
        public int poolSize;
    }

    public List<BulletPool> bulletPools = new List<BulletPool>();

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    public static BulletPoolManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (BulletPool pool in bulletPools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.bulletPrefab);
                obj.SetActive(false);
                obj.transform.SetParent(GameObject.Find("BulletPool").transform);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.bulletType, objectPool);
        }
    }

    // 타입에 해당하는 활성화된 오브젝트를 가져옴
    public GameObject GetObject(string bulletType)
    {
        if (poolDictionary.ContainsKey(bulletType))
        {
            Queue<GameObject> poolQueue = poolDictionary[bulletType];

            if (poolQueue.Count > 0)
            {
                GameObject getObj = poolQueue.Dequeue();
                getObj.SetActive(true);
                return getObj;
            }
            else
            {
                BulletPool pool = bulletPools.Find(t => t.bulletType == bulletType);
                if (pool.bulletPrefab != null)
                {
                    GameObject newObj = Instantiate(pool.bulletPrefab);
                    newObj.SetActive(true);
                    newObj.transform.SetParent(GameObject.Find("BulletPool").transform);
                    return newObj;
                }
                else
                {
                    Debug.LogError("No prefab found for monster type: " + bulletType);
                    return null;
                }
            }
        }
        else
        {
            Debug.LogError("Pool with type " + bulletType + " doesn't exist.");
        }

        return null;
    }

    // 사용한 오브젝트를 풀에 반환
    public void ReturnObject(string bulletType, GameObject obj)
    {
        if (poolDictionary.ContainsKey(bulletType))
        {
            obj.SetActive(false);
            poolDictionary[bulletType].Enqueue(obj);
        }
        else
        {
            Debug.LogError("Pool with type " + bulletType + " doesn't exist.");
        }
    }
}

