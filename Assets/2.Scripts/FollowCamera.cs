using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public OVRCameraRig cameraRig;
    public Vector3 uiOffset;

    private Transform centerEyeTransform;

    private void Start()
    {
        if (cameraRig != null)
        {
            // OVRCameraRig의 CenterEyeAnchor를 기준으로 설정
            centerEyeTransform = cameraRig.centerEyeAnchor;
        }
    }

    private void Update()
    {
        if (centerEyeTransform != null)
        {
            // 카메라의 위치에 오프셋을 더하여 UI의 위치를 설정
            transform.position = centerEyeTransform.position + centerEyeTransform.rotation * uiOffset;

            // UI가 카메라의 회전을 따라가도록 설정
            transform.rotation = centerEyeTransform.rotation;
        }
    }
}
