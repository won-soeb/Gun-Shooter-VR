using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : SingletonManager<MonsterPoolManager>
{
    public GameObject monsterObject;//풀링할 오브젝트
    public int poolSize = 0;//미리 생성할 개수
    private GameObject poolObjs;//큐에 넣을 오브젝트
    public Queue<GameObject> poolQueue = new Queue<GameObject>();//큐 - 넣은 순서대로 꺼내야 함

    private void Start()
    {
        //풀링할 오브젝트를 미리 생성 후 비활성화-큐에 넣음
        for (int i = 0; i < poolSize; i++)
        {
            poolObjs = Instantiate(monsterObject);
            poolObjs.SetActive(false);
            poolObjs.transform.SetParent(GameObject.Find("MonsterPool").transform);
            poolQueue.Enqueue(poolObjs);
        }
    }
    //활성화 로직
    public GameObject GetObject()
    {
        GameObject getObj = null;
        //큐에 오브젝트가 남아있으면
        if (poolQueue.Count > 0)
        {
            //큐에서 꺼낸다
            getObj = poolQueue.Dequeue();
            //getObj.transform.SetParent(GameObject.Find("MonsterPool").transform);
            getObj.SetActive(true);//활성화
        }
        return getObj;
    }
    //비활성화 로직
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
