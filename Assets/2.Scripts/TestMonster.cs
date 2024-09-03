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
            //�÷��̾� ���� ����
            Vector3 dir = player.transform.position - transform.position;
            //�÷��̾� �ٶ󺸱�
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            //�������� �ʵ��� �Ѵ�
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            //�÷��̾���� �Ÿ� Ȯ��
            if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
            {
            }
            else
            {
                //�÷��̾�� �̵�
                transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
            }
        }
    }
}
