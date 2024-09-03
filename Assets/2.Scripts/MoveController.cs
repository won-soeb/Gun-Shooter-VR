using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    public bool EnableLinearMovement = true;
    public bool EnableRotation = true;
    public bool HMDRotatesPlayer = true;
    public bool RotationEitherThumbstick = false;
    public float RotationAngle = 45.0f;
    public float Speed = 0.0f;
    private float SpeedUpRate = 1;
    public float speedUpPoint = 100;
    private bool isTeleportReady = false;

    public OVRCameraRig CameraRig;
    public GameObject teleportInteractor;

    private bool ReadyToSnapTurn;
    private Rigidbody _rigidbody;

    public event Action CameraUpdated;
    public event Action PreCharacterMove;

    Coroutine teleportRoutine = null;

    public Text debugText;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (CameraRig == null) CameraRig = GetComponentInChildren<OVRCameraRig>();
    }
    private void Start()
    {
        //teleportInteractor Ȱ��/��Ȱ�� ������ �̺�Ʈ�� ���
        teleportInteractor.GetComponent<TeleportState>().TeleportOn += () =>
        {
            isTeleportReady = true;
        };
        teleportInteractor.GetComponent<TeleportState>().TeleportOff += () =>
        {
            isTeleportReady = false;
            speedUpPoint = 0;
            GameManager.Instance.UpdateSpeedUpbar(speedUpPoint);
        };
    }
    private void FixedUpdate()
    {
        if (CameraUpdated != null) CameraUpdated();
        if (PreCharacterMove != null) PreCharacterMove();

        if (HMDRotatesPlayer) RotatePlayerToHMD();
        if (EnableLinearMovement) StickMovement();
        if (EnableRotation) SnapTurn();
        //�浹�� ȸ������ �ʵ��� ����
        _rigidbody.angularVelocity = Vector3.zero;
        //Debug.LogFormat("<color=yellow>speed : {0}</color>", isSpeedUp);
    }
    private void Update()
    {
        SpeedUp();
        //�ڷ���Ʈ �� �������� �ʱ�ȭ
        Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //�α� ���
        debugText.text = secondaryAxis.ToString();
        if (secondaryAxis.y > 0.9f || Input.GetKey(KeyCode.T))
        {
            if (speedUpPoint < 100) return;
            teleportInteractor.SetActive(true);
        }
        else
        {
            if (teleportRoutine == null)
            {
                teleportRoutine = StartCoroutine(OffTeleport());
            }
        }
    }
    IEnumerator OffTeleport()
    {
        yield return new WaitForSeconds(0.05f);
        teleportInteractor.SetActive(false);
        //yield return null;
        teleportRoutine = null;
    }
    void RotatePlayerToHMD()
    {
        Transform root = CameraRig.trackingSpace;
        Transform centerEye = CameraRig.centerEyeAnchor;

        Vector3 prevPos = root.position;
        Quaternion prevRot = root.rotation;

        transform.rotation = Quaternion.Euler(0.0f, centerEye.rotation.eulerAngles.y, 0.0f);

        root.position = prevPos;
        root.rotation = prevRot;
    }
    void StickMovement()
    {
        //�ڷ���Ʈ ���� �� �̵��� �� ����
        if (isTeleportReady) return;

        Quaternion ort = CameraRig.centerEyeAnchor.rotation;
        Vector3 ortEuler = ort.eulerAngles;
        ortEuler.z = ortEuler.x = 0f;
        ort = Quaternion.Euler(ortEuler);

        Vector3 moveDir = Vector3.zero;
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);//�޼� ��ƽ
        moveDir += ort * (primaryAxis.x * Vector3.right);
        moveDir += ort * (primaryAxis.y * Vector3.forward);
        //_rigidbody.MovePosition(_rigidbody.transform.position + moveDir * Speed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(_rigidbody.position + moveDir * Speed * SpeedUpRate * Time.fixedDeltaTime);
    }
    void SpeedUp()
    {
        //�ڷ���Ʈ ���� �� �̼� ���� �Ұ�
        if (isTeleportReady) return;
        //speedUp ��Ÿ�� ������
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || Input.GetKey(KeyCode.S))
        {
            if (speedUpPoint < 0)
            {
                SpeedUpRate = 1;
            }
            else
            {
                SpeedUpRate = 2;
                GameManager.Instance.UpdateSpeedUpbar(speedUpPoint-- / 100f);
            }
        }
        else
        {
            SpeedUpRate = 1;
            if (speedUpPoint > 100) return;
            GameManager.Instance.UpdateSpeedUpbar(speedUpPoint++ / 100f);
        }
    }
    void SnapTurn()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) ||
            (RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft)))
        {
            if (ReadyToSnapTurn)
            {
                ReadyToSnapTurn = false;
                transform.RotateAround(CameraRig.centerEyeAnchor.position, Vector3.up, -RotationAngle);
            }
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) ||
                 (RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight)))
        {
            if (ReadyToSnapTurn)
            {
                ReadyToSnapTurn = false;
                transform.RotateAround(CameraRig.centerEyeAnchor.position, Vector3.up, RotationAngle);
            }
        }
        else
        {
            ReadyToSnapTurn = true;
        }
    }
}
