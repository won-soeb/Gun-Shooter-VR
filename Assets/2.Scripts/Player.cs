using Oculus.Haptics;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int weaponNum = 0;
    public GameObject[] gunObjects;
    public GunData[] gunData;
    public HapticClip haptic;
    public ItemReceiver itemReceiver;

    public float maxHp = 100;
    private float currentHp;
    private HapticClipPlayer player;

    private AudioSource audioSource;
    public AudioClip[] clip;

    private void Awake()
    {
        currentHp = maxHp;
        player = new HapticClipPlayer(haptic);
        player.clip = haptic;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        itemReceiver.ReceiveItem += other =>
        {
            if (other.tag == "Item")
            {
                //GameManager.Instance.isGunRefillInfinity = true;
                Item itemType = other.GetComponent<Item>();

                switch (itemType.itemType)
                {
                    case Item.ItemType.Recovery:
                        //audioSource.PlayOneShot(clip[1]);
                        currentHp = maxHp;
                        GameManager.Instance.UpdateHpbar(maxHp);
                        break;
                    case Item.ItemType.Pistol:
                        audioSource.PlayOneShot(clip[0]);
                        gunData[0].currentAmmo = gunData[0].maxAmmo;
                        GameManager.Instance.UpdateAmmo(0);
                        break;
                    case Item.ItemType.ShotGun:
                        audioSource.PlayOneShot(clip[0]);
                        gunData[1].currentAmmo = gunData[1].maxAmmo;
                        GameManager.Instance.UpdateAmmo(1);
                        break;
                    case Item.ItemType.MachineGun:
                        audioSource.PlayOneShot(clip[0]);
                        gunData[2].currentAmmo = gunData[2].maxAmmo;
                        GameManager.Instance.UpdateAmmo(2);
                        break;
                }
                Destroy(other.gameObject);
            }
        };
        //게임 시작 시 탄환 수 리셋
        foreach (GunData data in gunData)
        {
            data.currentAmmo = data.maxAmmo;
        }
    }
    public void Damage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            GameManager.Instance.UpdateHpbar(currentHp / maxHp);
            GameManager.Instance.GameOver();
        }
        else
        {
            player.Play(Controller.Both);
            GameManager.Instance.UpdateHpbar(currentHp / maxHp);
        }
    }
    private void Update()
    {
        ChangeWeapon();
        //Reload();
        GameManager.Instance.UpdateAmmo(weaponNum);
    }

    private void Reload()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reload!");
            gunData[weaponNum].currentAmmo = gunData[weaponNum].maxAmmo;
        }
    }

    private void ChangeWeapon()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) || Input.GetKeyDown(KeyCode.G))
        {
            weaponNum++;
            weaponNum = (int)Mathf.Repeat(weaponNum, 3);
            gunObjects.ToList().ForEach(gun => gun.SetActive(gun == gunObjects[weaponNum]));
            GameManager.Instance.UpdateWeaponName(weaponNum);
        }
    }
    public float PlayerAttack => gunData[weaponNum].damage;
}
