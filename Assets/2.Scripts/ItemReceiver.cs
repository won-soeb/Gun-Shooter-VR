using System;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    public Action<Collider> ReceiveItem;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name + "과 충돌함");
        ReceiveItem?.Invoke(other);
    }
}
