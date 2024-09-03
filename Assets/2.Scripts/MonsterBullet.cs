using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    public float bulletSpeed = 3f;
    private Player player;
    private Vector3 dir;

    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            dir = player.transform.position - transform.position;
            //Debug.LogFormat("<color=yellow>dir : {0}</color>", dir);
        }
    }
    private void Update()
    {
        if (player != null)
        {
            transform.Translate(dir.normalized * bulletSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(player.transform.position, transform.position) > 20)
        {
            //BulletPoolManager.Instance.ReturnObject("MonsterBullet", gameObject);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.Damage(GameManager.Instance.flymonsterAttack);
            //BulletPoolManager.Instance.ReturnObject("MonsterBullet", gameObject);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "DeadZone" || collision.gameObject.tag == "Barrier")
        {
            //BulletPoolManager.Instance.ReturnObject("MonsterBullet", gameObject);
            Destroy(gameObject);
        }
    }
}
