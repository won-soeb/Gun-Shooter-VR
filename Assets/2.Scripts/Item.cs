using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Pistol, ShotGun, MachineGun, Recovery }
    public ItemType itemType;
    public bool rotate = false;
    //�������� �ı��Ǿ��ٴ� ���� �˷��� �븮��
    public Action OnDestroyed;

    private void Start()
    {
        //rotate = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f
            , transform.position.z);
    }
    private void Update()
    {
        if (rotate)
        {
            //���� �����Ǵ� �������� y�� �������� ȸ���Ѵ�
            transform.Rotate(0, 3, 0);
        }
        else
        {
            //���Ϳ��� �����Ǵ� �������� ���Ʒ��� �̵��Ѵ�
            transform.Translate(0, Mathf.Sin(Time.time * 2.5f) / 300, 0);
        }
    }
    private void OnDisable()//OnDestroy�� �ȵ�
    {
        OnDestroyed?.Invoke();
        //if (OnDestroyed != null) OnDestroyed();// �̷��Ե� ��
    }
}
