using System;
using UnityEngine;

public class TeleportState : MonoBehaviour
{
    public Action TeleportOn;
    public Action TeleportOff;
    private void OnEnable()
    {
        TeleportOn?.Invoke();
    }
    private void OnDisable()
    {
        TeleportOff?.Invoke();
    }
}
