using System;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    public Action<Collider> ReceiveItem;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name + "�� �浹��");
        ReceiveItem?.Invoke(other);
    }
}
