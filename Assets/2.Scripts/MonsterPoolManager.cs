using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolManager : MonoBehaviour
{
    public GameObject monsterObject;//Ǯ���� ������Ʈ
    public int poolSize = 0;//�̸� ������ ����
    private GameObject poolObjs;//ť�� ���� ������Ʈ
    public Queue<GameObject> poolQueue = new Queue<GameObject>();//ť - ���� ������� ������ ��

    public static MonsterPoolManager Instance = new MonsterPoolManager();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //Ǯ���� ������Ʈ�� �̸� ���� �� ��Ȱ��ȭ-ť�� ����
        for (int i = 0; i < poolSize; i++)
        {
            poolObjs = Instantiate(monsterObject);
            poolObjs.SetActive(false);
            poolObjs.transform.SetParent(GameObject.Find("MonsterPool").transform);
            poolQueue.Enqueue(poolObjs);
        }
    }
    //Ȱ��ȭ ����
    public GameObject GetObject()
    {
        GameObject getObj = null;
        //ť�� ������Ʈ�� ����������
        if (poolQueue.Count > 0)
        {
            //ť���� ������
            getObj = poolQueue.Dequeue();
            //getObj.transform.SetParent(GameObject.Find("MonsterPool").transform);
            getObj.SetActive(true);//Ȱ��ȭ
        }
        return getObj;
    }
    //��Ȱ��ȭ ����
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
