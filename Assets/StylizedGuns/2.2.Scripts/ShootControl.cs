using UnityEngine;

public class ShootControl : MonoBehaviour
{
    public string bulletType;
    public Transform shootPoint;
    public GameObject bullet;
    public Animator gunAnimator;
    [Range(0, 1)]
    public float disperseBullet;
    public GunData gunData;

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.Space))
        {
            gunAnimator.SetBool("IsShooting", true);
            //Debug.LogFormat("<color=yellow>Shot!</color>");
        }
    }
    public void Shoot()
    {
        StopShootingAn();
        //�Ѿ��� �ٴڳ��� �߻����� ����
        if (gunData.currentAmmo <= 0) return;
        //�Ѿ� �߻�
        GameObject newBullet = BulletPoolManager.Instance.GetObject(bulletType);
        if (newBullet != null)
        {
            newBullet.transform.position = shootPoint.position;
            newBullet.transform.rotation = shootPoint.rotation;
            newBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;//�ӵ� �ʱ�ȭ
            newBullet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * 2000 +
                new Vector3(Random.Range(disperseBullet, -disperseBullet), Random.Range(disperseBullet, -disperseBullet), 0) * 100);
        }
        //���� źȯ ����
        gunData.currentAmmo--;
        StopShootingAn();
    }

    public void StopShootingAn()
    {
        gunAnimator.SetBool("IsShooting", false);
    }
}
