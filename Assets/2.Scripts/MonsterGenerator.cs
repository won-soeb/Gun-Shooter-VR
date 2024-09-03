using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject[] genPosition;
    //public GameObject monsterObject;
    public float genTime = 1;
    private int randNum = 0;
    private void Update()
    {
        GenMonster();
    }
    float timer = 0;
    private void GenMonster()
    {
        timer += Time.deltaTime;
        if (timer >= genTime)
        {
            timer = 0;
            randNum = Random.Range(0, genPosition.Length);
            GameObject monster = MonsterPoolManager.Instance.GetObject();
            //GameObject monster = Instantiate(monsterObject);
            if (monster != null)
            {
                monster.transform.position = genPosition[randNum].transform.position;
            }
        }
    }
}
