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
            // OVRCameraRig�� CenterEyeAnchor�� �������� ����
            centerEyeTransform = cameraRig.centerEyeAnchor;
        }
    }

    private void Update()
    {
        if (centerEyeTransform != null)
        {
            // ī�޶��� ��ġ�� �������� ���Ͽ� UI�� ��ġ�� ����
            transform.position = centerEyeTransform.position + centerEyeTransform.rotation * uiOffset;

            // UI�� ī�޶��� ȸ���� ���󰡵��� ����
            transform.rotation = centerEyeTransform.rotation;
        }
    }
}
