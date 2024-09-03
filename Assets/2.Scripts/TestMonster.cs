using UnityEngine;

public class TestMonster : MonoBehaviour
{
    private Player player;
    public float AttackDistance = 1f;
    public float moveSpeed = 3;
    private void Update()
    {
        if (player != null)
        {
            //플레이어 방향 추적
            Vector3 dir = player.transform.position - transform.position;
            //플레이어 바라보기
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            //기울어지지 않도록 한다
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            //플레이어와의 거리 확보
            if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
            {
            }
            else
            {
                //플레이어에게 이동
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
            }
        }
    }
}
