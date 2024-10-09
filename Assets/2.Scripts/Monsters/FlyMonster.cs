using System.Collections;
using UnityEngine;

public class FlyMonster : Monster
{
    Animator anim;
    Player player;
    AudioSource audioSource;
    [Header("Move, Attack, Damage, Die")]
    public AudioClip[] clips;
    public GameObject monsterBullet;
    public Transform bulletPos;
    public Rigidbody rigid;
    public float AttackDistance = 10;
    public float moveSpeed = 3;
    public float increaseHp = 2;
    private float hp = 10;
    private string animationName;
    private bool isDead;
    Coroutine takeoffRoutine;
    public Transform rayPosition;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        //리젠시 체력과 공격력 증가
        GameManager.Instance.flymonsterMaxHp += increaseHp;
        hp = GameManager.Instance.flymonsterMaxHp;
        GameManager.Instance.flymonsterAttack++;
        player = FindAnyObjectByType<Player>();
        isDead = false;

        if (takeoffRoutine != null)
        {
            StopCoroutine(takeoffRoutine);
        }
        takeoffRoutine = StartCoroutine(StartTakeOff(3));
    }
    private void OnDisable()
    {
        GameManager.Instance.UpdateScore(30);
    }
    //private void OnDestroy()
    //{
    //    GameManager.Instance.UpdateScore(30);      
    //}
    private void Update()
    {
        //현재 애니메이션 상태 확인(클립 이름)
        animationName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        //Debug.Log("몬스터 상태 : " + animationName);

        if (player != null)
        {
            //플레이어 방향 추적
            Vector3 dir = player.transform.position - transform.position;
            //플레이어 바라보기
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);

            //사망 여부
            if (!isDead)
            {
                //벽 감지
                Debug.DrawRay(rayPosition.position, transform.forward * 2, Color.red);
                if (Physics.Raycast(rayPosition.position, transform.forward, 2, LayerMask.GetMask("Barrier")))
                {
                    Attack(false);
                    return;
                }
                //플레이어와의 거리 확보
                if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
                {
                    Attack(true);
                }
                else
                {
                    //플레이어에게 이동
                    Attack(false);
                    transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                //추락
                transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
                //transform.position += Vector3.down * 10 * moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            }
        }
        //애니메이션 오류 처리 - 추락 모션이 비정상적으로 실행되거나 땅에 떨어져도 죽지 않을 시
        DieException();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (animationName == "Falling" &&
            (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Barrier"))
        {
            anim.SetTrigger("DieOnGround");
            Invoke("Release", 0.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //플레이어 총알에 맞은 경우
        if (other.tag == "Bullet")
        {
            Damage(player.PlayerAttack);
            PlaySound(2);
        }
        if (other.tag == "Player")
        {
            Attack(true);
        }
        if (other.tag == "Ground")
        {
            //Debug.Log("몬스터 착륙");
            anim.SetBool("Fly", false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            //Debug.Log("몬스터 이륙");
            anim.SetBool("Fly", true);
        }
        if (other.tag == "Player")
        {
            Attack(false);//해결해야됨
            StartCoroutine(StartTakeOff(0.1f));
        }
    }
    public override void Attack(bool isAttack)
    {
        //Debug.Log("몬스터 공격");
        if (animationName == "Fly" || animationName == "FlyingBiteAttack")
        {
            anim.SetBool("AttackOnFly", isAttack);
        }
        if (animationName == "Move")
        {
            int num = Random.Range(1, 3);
            anim.SetBool("Attack" + num, isAttack);
        }
    }
    public override void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
            hp = 0;
        }
        else
        {
            //Debug.Log("데미지를 입음");
            if (animationName == "Fly" || animationName == "FlyingBiteAttack")
            {
                anim.SetTrigger("HitOnFly");
            }
            else
            {
                int num = Random.Range(1, 3);
                anim.SetTrigger("Hit" + num);
            }
        }
    }
    public override void Die()
    {
        PlaySound(3);
        //Debug.Log("몬스터 처치");
        isDead = true;
        if (takeoffRoutine != null)
        {
            StopCoroutine(takeoffRoutine);
        }
        if (animationName == "Fly" || animationName == "FlyingBiteAttack")
        {
            anim.SetBool("AttackOnFly", false);
            anim.SetTrigger("DieOnFly");
        }
        else if (animationName == "Move" || animationName == "Attack1" || animationName == "Attack2")
        {
            anim.SetTrigger("Die");
            Invoke("Release", 1);
        }
    }
    private void DieException()
    {
        if (hp <= 0 && rigid.velocity.y >= 0 && (animationName == "Falling" || animationName == "IdleBreathe"))
        {
            Invoke("Release", 0.5f);
        }
    }
    private void Release()
    {
        //MonsterPoolManager.Instance.ReturnObject(gameObject);
        Destroy(gameObject);
    }
    IEnumerator StartTakeOff(float takeOffTime)
    {
        PlaySound(0);
        float timer = 0;
        anim.SetBool("Fly", true);
        while (timer <= takeOffTime)
        {
            timer += Time.deltaTime;
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            if (timer == takeOffTime)
            {
                takeoffRoutine = null;
            }
            yield return null;
        }
    }
    //몬스터 총알 발사
    private void Shot()
    {
        if (isDead) return;
        PlaySound(1);
        Instantiate(monsterBullet, bulletPos.position, Quaternion.identity);
        //GameObject bullet = BulletPoolManager.Instance.GetObject("MonsterBullet");
        //if (bullet != null)
        //{
        //    bullet.transform.position = bulletPos.position;
        //}
    }
    private void PlaySound(int clip)
    {
        audioSource.clip = clips[clip];
        audioSource.Play();
    }
}
