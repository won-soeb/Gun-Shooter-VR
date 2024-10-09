using UnityEngine;

public class Skeleton : Monster
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
        //이벤트 - 몬스터 무기에 접촉할 경우 사운드 재생
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
        //리젠시 체력과 공격력 증가
        GameManager.Instance.skeletonHp += increaseHp;
        hp = GameManager.Instance.skeletonHp;//몬스터 체력 초기화
        GameManager.Instance.skeletonAttack += increaseAttack;
    }
    private void Update()
    {
        if (player != null)
        {
            //플레이어 방향 추적
            Vector3 dir = player.transform.position - transform.position;
            //플레이어 바라보기
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            //기울어지지 않도록 한다
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            //사망하면 움직이지 않음
            if (isDead) return;
            //벽 감지
            Debug.DrawRay(rayPosition.position, transform.forward * 5, Color.red);
            //Debug.Log("벽 감지 : "+ Physics.Raycast(rayPosition.position, transform.forward, 5, LayerMask.GetMask("Barrier")));
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

            //플레이어와의 거리 확보
            if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
            {
                anim.SetBool("Move", false);
                //PlaySound(clip[0], false);
                Attack(true);
            }
            else
            {
                Attack(false);
                //플레이어에게 이동
                anim.SetBool("Move", true);
                transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
                //PlaySound(clip[0], true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //플레이어 총알에 맞은 경우
        if (other.tag == "Bullet")
        {
            Damage(player.PlayerAttack);
        }
    }
    public override void Attack(bool isAttack)
    {
        if (isDead) return;
        //Debug.Log("몬스터 공격");
        anim.SetBool("Attack", isAttack);
    }
    public override void Damage(float damage)
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
            //Debug.Log("데미지를 입음");
            //PlaySound(clip[0], false);
            PlaySound(clip[2], true);
            anim.SetTrigger("Hit");
        }
    }
    public override void Die()
    {
        if (isDead) return;

        isDead = true;
        //Debug.Log("몬스터 처치");
        sword.GetComponent<BoxCollider>().enabled = false;

        anim.SetTrigger("Die");
        Invoke("Release", 2);
    }
    private void Release()
    {
        //Destroy(gameObject);
        MonsterPoolManager.Instance.ReturnObject(gameObject);
        //점수 추가
        GameManager.Instance.UpdateScore(10);
        //죽은 자리에 아이템 생성
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


