using UnityEngine;

public class Skeleton : MonoBehaviour, IMonster
{
    public Animator anim;
    private Player player;
    public float AttackDistance = 1f;
    public float moveSpeed = 3;
    public float increaseHp = 1;
    public float increaseAttack = 0.1f;
    private float hp;
    private bool isDead;
    public GameObject sword;
    public Transform rayPosition;
    private AudioSource audioSource;
    [Header("Move, Attack, Damage, Die")]
    public AudioClip[] clip;//move, attack, damage, die
    private void Start()
    {
        //�̺�Ʈ - ���� ���⿡ ������ ��� ���� ���
        sword.GetComponent<MonsterSword>().PlaySound += () =>
        {
            //PlaySound(clip[0], false);
            PlaySound(clip[1], true);
        };
    }
    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
        audioSource = GetComponent<AudioSource>();
        isDead = false;
        //audioSource.loop = true;
        sword.GetComponent<BoxCollider>().enabled = true;
        //������ ü�°� ���ݷ� ����
        GameManager.Instance.skeletonHp += increaseHp;
        hp = GameManager.Instance.skeletonHp;//���� ü�� �ʱ�ȭ
        GameManager.Instance.skeletonAttack += increaseAttack;
    }
    private void Update()
    {
        if (player != null)
        {
            //�÷��̾� ���� ����
            Vector3 dir = player.transform.position - transform.position;
            //�÷��̾� �ٶ󺸱�
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            //�������� �ʵ��� �Ѵ�
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            //����ϸ� �������� ����
            if (isDead) return;
            //�� ����
            Debug.DrawRay(rayPosition.position, transform.forward * 5, Color.red);
            //Debug.Log("�� ���� : "+ Physics.Raycast(rayPosition.position, transform.forward, 5, LayerMask.GetMask("Barrier")));
            if (Physics.Raycast(rayPosition.position, transform.forward, 5, LayerMask.GetMask("Barrier")))
            {
                anim.SetBool("Move", false);
                //PlaySound(clip[0], false);
                return;
            }
            else
            {
                anim.SetBool("Move", true);
                //PlaySound(clip[0], true);
            }

            //�÷��̾���� �Ÿ� Ȯ��
            if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
            {
                anim.SetBool("Move", false);
                //PlaySound(clip[0], false);
                Attack(true);
            }
            else
            {
                Attack(false);
                //�÷��̾�� �̵�
                anim.SetBool("Move", true);
                transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
                //PlaySound(clip[0], true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //�÷��̾� �Ѿ˿� ���� ���
        if (other.tag == "Bullet")
        {
            Damage(player.PlayerAttack);
        }
    }
    public void Attack(bool isAttack)
    {
        if (isDead) return;
        //Debug.Log("���� ����");
        anim.SetBool("Attack", isAttack);
    }
    public void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            audioSource.loop = false;
            PlaySound(clip[3], true);
            Die();
            hp = 0;
        }
        else
        {
            //Debug.Log("�������� ����");
            //PlaySound(clip[0], false);
            PlaySound(clip[2], true);
            anim.SetTrigger("Hit");
        }
    }
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        //Debug.Log("���� óġ");
        sword.GetComponent<BoxCollider>().enabled = false;

        anim.SetTrigger("Die");
        Invoke("Release", 2);
    }
    private void Release()
    {
        //Destroy(gameObject);
        MonsterPoolManager.Instance.ReturnObject(gameObject);
        //���� �߰�
        GameManager.Instance.UpdateScore(10);
        //���� �ڸ��� ������ ����
        GameObject item = ItemManager.Instance.CreateItem();
        if (item != null)
        {
            item.transform.position = transform.position;
            item.transform.rotation = transform.rotation;
        }
    }
    private void PlaySound(AudioClip clip, bool isPlay)
    {
        if (audioSource != null && clip != null)
        {
            if (isPlay)
            {
                if (audioSource.isPlaying) return;
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}


