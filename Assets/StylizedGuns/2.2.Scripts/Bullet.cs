using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string bulletType;
    public GameObject explotion;
    public Rigidbody rigid;
    GameObject lastExplotion;

    private void OnTriggerEnter(Collider other)
    {
        lastExplotion = Instantiate(explotion, transform.position, transform.rotation);
        BulletPoolManager.Instance.ReturnObject(bulletType, gameObject);
        Destroy(lastExplotion, 1f);
    }
}
