using UnityEngine;
using Oculus.Haptics;

public class HapticTest : MonoBehaviour
{
    public HapticClip clip1;
    public HapticClip clip2;
    private HapticClipPlayer player;
    void Awake()
    {
        player = new HapticClipPlayer(clip1);
    }
    public void PlayHapticClip1()
    {
        player.clip = clip1; 
        player.Play(Controller.Left);
    }
    public void PlayHapticClip2()
    { 
        player.clip = clip2;
        player.Play(Controller.Right);
    }
    public void StopHaptics()
    {
        player.Stop();
    }
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("왼손 트리거");
            PlayHapticClip1();
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("오른손 트리거");
            PlayHapticClip2();
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            StopHaptics();
        }
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            StopHaptics();
        }
    }
}

