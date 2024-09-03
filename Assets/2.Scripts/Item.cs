using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Pistol, ShotGun, MachineGun, Recovery }
    public ItemType itemType;
    public bool rotate = false;
    //아이템이 파괴되었다는 것을 알려줄 대리자
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
            //무한 리젠되는 아이템은 y축 기준으로 회전한다
            transform.Rotate(0, 3, 0);
        }
        else
        {
            //몬스터에서 리젠되는 아이템은 위아래로 이동한다
            transform.Translate(0, Mathf.Sin(Time.time * 2.5f) / 300, 0);
        }
    }
    private void OnDisable()//OnDestroy는 안됨
    {
        OnDestroyed?.Invoke();
        //if (OnDestroyed != null) OnDestroyed();// 이렇게도 씀
    }
}
